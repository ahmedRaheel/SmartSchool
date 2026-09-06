using SmartSchool.Modules.AICore.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json;
using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.Modules.AICore.Cag;
using SmartSchool.Modules.AICore.Rag.Ollama;
using Microsoft.Extensions.Options;
using SmartSchool.Modules.AICore.Features.AiExecutionLog;
using SmartSchool.Modules.AICore.Features.KnowledgeChunk;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AICore.Features;

public static class OperationalAiCoreEndpoints
{
    public sealed record IndexKnowledgeRequest(Guid? TenantId, Guid CollectionId, Guid? DocumentId, string Code, string Name, string Content, string[]? Tags);
    public sealed record ExecuteRequest(Guid? TenantId, string Assistant, string Prompt, string[]? Collections);
    private sealed record Hit(Guid Id,string DocumentName,string Collection,string Content,double Score);

    public static IEndpointRouteBuilder MapOperationalAiCoreEndpoints(this IEndpointRouteBuilder e)
    {
        var g=e.MapGroup("/api/aicore").WithTags("AICore Operations").RequireAuthorization();
        g.MapPost("/knowledge/index",IndexAsync).RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantAdmin);
        g.MapPost("/execute",ExecuteAsync);
        g.MapGet("/health",HealthAsync);
        return e;
    }

    private static async Task<IResult> IndexAsync(IndexKnowledgeRequest r,ITenantScope scope,OperationalAiCoreEndpointsKnowledgeChunkWriteData command,
        IDbConnectionFactory db,IOllamaClient ollama,IIntegrationEventPublisher events,CancellationToken ct)
    {
        var tenant = scope.IsSuperAdmin? r.TenantId:
                     scope.Resolve(r.TenantId);
        if(!tenant.HasValue)
            return Results.BadRequest(new{message="A tenant is required."});

        if (string.IsNullOrWhiteSpace(r.Content))
            return Results.BadRequest(new{message="Content is required."});
        var vector = await ollama.EmbedAsync(r.Content,ct);
        var entity= KnowledgeChunkEntity.Create(tenant.Value,r.Code,r.Name,JsonSerializer.Serialize(new{r.CollectionId,r.DocumentId,r.Content,r.Tags}));
        await command.AddAsync(entity,ct);
        const string sql="""

            INSERT INTO ai_core.rag_knowledge_chunk(id,
                       tenant_id,
                       collection,document_name,
                       content,embedding,
                       created_at,is_active
                       )
            VALUES(
                 @Id,
                 @Tenant,
                 @Collection,
                 @Name,
                 @Content,
                 CAST(@Vector AS vector),
                 CURRENT_TIMESTAMP,TRUE)
                 ON CONFLICT (id) DO UPDATE SET content=EXCLUDED.content,embedding=EXCLUDED.embedding;

            """;
        await using var cn=await db.OpenConnectionAsync(ct);
        await cn.ExecuteAsync(new CommandDefinition(sql,new{Id=entity.KnowledgeChunkId,Tenant=tenant.Value,Collection=r.CollectionId.ToString(),r.Name,r.Content,Vector=Literal(vector)},cancellationToken:ct));
        await events.PublishAsync(KafkaTopics.RagDocumentIngestionRequested,new{tenantId=tenant.Value,chunkId=entity.KnowledgeChunkId,r.CollectionId,r.DocumentId},ct);
        return Results.Created($"/api/aicore/knowledge-chunk/{entity.KnowledgeChunkId}",new{Id=entity.KnowledgeChunkId,TenantId=tenant.Value,indexed=true});
    }

    private static async Task<IResult> ExecuteAsync(ExecuteRequest r,ITenantScope scope,IDbConnectionFactory db,
        IOllamaClient ollama,IOptionsMonitor<OllamaRagOptions> options,OperationalAiCoreEndpointsAiExecutionLogWriteData logs,IIntegrationEventPublisher events,TimeProvider timeProvider,CancellationToken ct)
    {
        var tenant = scope.IsSuperAdmin ? r.TenantId:scope.Resolve(r.TenantId);
        if(!tenant.HasValue)
            return Results.BadRequest(new{message="A tenant is required."});
        var vector = await ollama.EmbedAsync(r.Prompt,ct);
        var collections = r.Collections is { Length:>0 } ? r.Collections:["learning","academic","policy","operations","admissions"];
        const string sql="""

            SELECT id AS "Id",document_name AS "DocumentName",collection AS "Collection",content AS "Content",
            1-(embedding <=> CAST(@Vector AS vector)) AS "Score" FROM ai_core.rag_knowledge_chunk
            WHERE tenant_id=@Tenant AND is_active=TRUE AND collection=ANY(@Collections)
            ORDER BY embedding <=> CAST(@Vector AS vector) LIMIT @TopK;

            """;
        await using var cn=await db.OpenConnectionAsync(ct);
        var hits=(await cn.QueryAsync<Hit>(new CommandDefinition(sql,new{Tenant=tenant.Value,Collections=collections,Vector=Literal(vector),TopK=options.CurrentValue.TopK},cancellationToken:ct))).ToArray();
        var context=string.Join("\n\n",hits.Select((h,i)=>$"[{i+1}] {h.DocumentName}\n{h.Content}"));
        var generated = await ollama.GenerateAsync($"You are SmartSchool {r.Assistant}. Use only authorized retrieved context and cite sources.\nCONTEXT:\n{context}\nQUESTION:\n{r.Prompt}", ct);
        var model = generated.Model;
        var answer = generated.Answer;
        var log=AiExecutionLogEntity.Create(tenant.Value,$"AI-{timeProvider.GetUtcNow():yyyyMMddHHmmssfff}",r.Assistant,
            JsonSerializer.Serialize(new{r.Prompt,answer,model,citations=hits.Select(x=>new{x.Id,x.DocumentName,x.Score})}));
        await logs.AddAsync(log,ct);
        await events.PublishAsync(KafkaTopics.ChatbotQuestionAsked,new{tenantId=tenant.Value,assistant=r.Assistant,executionId=log.AiExecutionLogId},ct);
        return Results.Ok(new{executionId=log.AiExecutionLogId,answer,model,citations=hits.Select(x=>new{x.Id,x.DocumentName,x.Collection,x.Score})});
    }

    private static async Task<IResult> HealthAsync(
        IHttpClientFactory clients,
        IOptionsMonitor<OllamaRagOptions> options,
        CancellationToken cancellationToken)
    {
        try
        {
            var client = clients.CreateClient("Ollama");
            using var response = await client.GetAsync("api/tags", cancellationToken);
            return response.IsSuccessStatusCode
                ? Results.Ok(new { ollama = true, pgvector = true, model = options.CurrentValue.ChatModel })
                : Results.Json(new { ollama = false, status = (int)response.StatusCode }, statusCode: 503);
        }
        catch (HttpRequestException exception)
        {
            return Results.Json(new { ollama = false, error = exception.Message }, statusCode: 503);
        }
    }

    private static string Literal(IEnumerable<float> x)=>"["+string.Join(",",x.Select(v=>v.ToString(CultureInfo.InvariantCulture)))+"]";
}

/// <summary>
/// Feature-owned data access for OperationalAiCoreEndpoints. Do not share across slices.
/// </summary>
internal sealed class OperationalAiCoreEndpointsKnowledgeChunkWriteData(IAICoreDbContext dbContext)
{
    public async Task AddAsync(
        KnowledgeChunkEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.KnowledgeChunks
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

/// <summary>
/// Feature-owned data access for OperationalAiCoreEndpoints. Do not share across slices.
/// </summary>
internal sealed class OperationalAiCoreEndpointsAiExecutionLogWriteData(IAICoreDbContext dbContext)
{
    public async Task AddAsync(
        AiExecutionLogEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.AiExecutionLogs
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
