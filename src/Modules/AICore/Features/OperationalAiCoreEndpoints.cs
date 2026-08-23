using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using Dapper;
using Microsoft.Extensions.Caching.Distributed;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.Modules.AICore.Persistence;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AICore.Features;

public static class OperationalAiCoreEndpoints
{
    public sealed record IndexKnowledgeRequest(Guid? TenantId, Guid CollectionId, Guid? DocumentId, string Code, string Name, string Content, string[]? Tags);
    public sealed record ExecuteRequest(Guid? TenantId, string Assistant, string Prompt, string[]? Collections);
    private sealed record EmbeddingResponse(float[] Embedding);
    private sealed record GenerateResponse(string Response);
    private sealed record Hit(Guid Id,string DocumentName,string Collection,string Content,double Score);

    public static IEndpointRouteBuilder MapOperationalAiCoreEndpoints(this IEndpointRouteBuilder e)
    {
        var g=e.MapGroup("/api/aicore").WithTags("AICore Operations").RequireAuthorization();
        g.MapPost("/knowledge/index",IndexAsync).RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantAdmin);
        g.MapPost("/execute",ExecuteAsync);
        g.MapGet("/health",HealthAsync);
        return e;
    }

    private static async Task<IResult> IndexAsync(IndexKnowledgeRequest r,ITenantScope scope,IKnowledgeChunkCommand command,
        IDbConnectionFactory db,IHttpClientFactory clients,IConfiguration cfg,IIntegrationEventPublisher events,CancellationToken ct)
    {
        var tenant=scope.IsSuperAdmin?r.TenantId:scope.Resolve(r.TenantId);
        if(!tenant.HasValue)return Results.BadRequest(new{message="A tenant is required."});
        if(string.IsNullOrWhiteSpace(r.Content))return Results.BadRequest(new{message="Content is required."});
        var vector=await Embed(r.Content,clients,cfg,ct);
        var entity=KnowledgeChunkEntity.Create(tenant.Value,r.Code,r.Name,JsonSerializer.Serialize(new{r.CollectionId,r.DocumentId,r.Content,r.Tags}));
        await command.AddAsync(entity,ct);
        const string sql="""
			
			INSERT INTO ai_core.rag_knowledge_chunk
			(id,tenant_id,collection,document_name,content,embedding,created_at,is_active)			
			VALUES(@Id,@Tenant,@Collection,@Name,@Content,CAST(@Vector AS vector),CURRENT_TIMESTAMP,TRUE)
			ON CONFLICT (id) DO UPDATE SET content=EXCLUDED.content,embedding=EXCLUDED.embedding;
			
			""";
        await using var cn=await db.OpenConnectionAsync(ct);
        await cn.ExecuteAsync(new CommandDefinition(sql,new{entity.Id,Tenant=tenant.Value,Collection=r.CollectionId.ToString(),r.Name,r.Content,Vector=Literal(vector)},cancellationToken:ct));
        await events.PublishAsync(KafkaTopics.RagDocumentIngestionRequested,new{tenantId=tenant.Value,chunkId=entity.Id,r.CollectionId,r.DocumentId},ct);
        return Results.Created($"/api/aicore/knowledge-chunk/{entity.Id}",new{entity.Id,TenantId=tenant.Value,indexed=true});
    }

    private static async Task<IResult> ExecuteAsync(ExecuteRequest r,ITenantScope scope,IDbConnectionFactory db,
        IHttpClientFactory clients,IConfiguration cfg,IAiExecutionLogCommand logs,IIntegrationEventPublisher events,CancellationToken ct)
    {
        var tenant=scope.IsSuperAdmin?r.TenantId:scope.Resolve(r.TenantId);
        if(!tenant.HasValue)return Results.BadRequest(new{message="A tenant is required."});
        var vector=await Embed(r.Prompt,clients,cfg,ct);
        var collections=r.Collections is {Length:>0}?r.Collections:["learning","academic","policy","operations","admissions"];
        const string sql="""
			
			SELECT id AS "Id",
					document_name AS "DocumentName",
					  collection AS "Collection",
					  content AS "Content",
					  1-(embedding <=> CAST(@Vector AS vector)) AS "Score" 
				  FROM ai_core.rag_knowledge_chunk 
				  WHERE 
						tenant_id=@Tenant AND is_active=TRUE AND collection=ANY(@Collections)
						ORDER BY embedding <=> CAST(@Vector AS vector) LIMIT @TopK;
			
			""";
        await using var cn=await db.OpenConnectionAsync(ct);
        var hits=(await cn.QueryAsync<Hit>(new CommandDefinition(sql,new{Tenant=tenant.Value,Collections=collections,Vector=Literal(vector),TopK=cfg.GetValue("AI:Ollama:TopK",5)},cancellationToken:ct))).ToArray();
        var context=string.Join("\n\n",hits.Select((h,i)=>$"[{i+1}] {h.DocumentName}\n{h.Content}"));
        var model=cfg["AI:Ollama:ChatModel"]??"llama3.2";
        var answer=await Generate($"You are SmartSchool {r.Assistant}. Use only authorized retrieved context and cite sources.\nCONTEXT:\n{context}\nQUESTION:\n{r.Prompt}",clients,cfg,ct);
        var log=AiExecutionLogEntity.Create(tenant.Value,$"AI-{DateTime.UtcNow:yyyyMMddHHmmssfff}",r.Assistant,
            JsonSerializer.Serialize(new{r.Prompt,answer,model,citations=hits.Select(x=>new{x.Id,x.DocumentName,x.Score})}));
        await logs.AddAsync(log,ct);
        await events.PublishAsync(KafkaTopics.ChatbotQuestionAsked,new{tenantId=tenant.Value,assistant=r.Assistant,executionId=log.Id},ct);
        return Results.Ok(new{executionId=log.Id,answer,model,citations=hits.Select(x=>new{x.Id,x.DocumentName,x.Collection,x.Score})});
    }

    private static async Task<IResult> HealthAsync(IHttpClientFactory clients,IConfiguration cfg,CancellationToken ct)
    {
        try{var h=clients.CreateClient();h.BaseAddress=new Uri(Base(cfg));var r=await h.GetAsync("api/tags",ct);return Results.Ok(new{ollama=r.IsSuccessStatusCode,pgvector=true});}
        catch(Exception ex){return Results.Json(new{ollama=false,error=ex.Message},statusCode:503);}
    }
    private static string Base(IConfiguration c)=>(c["AI:Ollama:BaseUrl"]??"http://host.docker.internal:11434").TrimEnd('/')+"/";
    private static async Task<float[]> Embed(string text,IHttpClientFactory clients,IConfiguration cfg,CancellationToken ct){var h=clients.CreateClient();h.BaseAddress=new Uri(Base(cfg));var r=await h.PostAsJsonAsync("api/embeddings",new{model=cfg["AI:Ollama:EmbeddingModel"]??"nomic-embed-text",prompt=text},ct);r.EnsureSuccessStatusCode();return (await r.Content.ReadFromJsonAsync<EmbeddingResponse>(cancellationToken:ct))?.Embedding??throw new InvalidOperationException("No embedding.");}
    private static async Task<string> Generate(string prompt,IHttpClientFactory clients,IConfiguration cfg,CancellationToken ct){var h=clients.CreateClient();h.BaseAddress=new Uri(Base(cfg));var r=await h.PostAsJsonAsync("api/generate",new{model=cfg["AI:Ollama:ChatModel"]??"llama3.2",prompt,stream=false},ct);r.EnsureSuccessStatusCode();return (await r.Content.ReadFromJsonAsync<GenerateResponse>(cancellationToken:ct))?.Response??"";}
    private static string Literal(IEnumerable<float> x)=>"["+string.Join(",",x.Select(v=>v.ToString(CultureInfo.InvariantCulture)))+"]";
}
