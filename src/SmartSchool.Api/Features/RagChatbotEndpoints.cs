using System.Security.Claims;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using Dapper;
using Microsoft.Extensions.Caching.Distributed;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Application.Identity;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Api.Features;

public static class RagChatbotEndpoints
{
    public sealed record IndexRequest(Guid? TenantId, string Collection, string DocumentName, string Content);
    public sealed record AskRequest(Guid? TenantId, string Question, Guid? SchoolId = null);
    public sealed record Citation(Guid Id, string DocumentName, string Collection, double Score);
    public sealed record AskResponse(string Bot, string Answer, string Model, IReadOnlyCollection<Citation> Citations);
    private sealed record OllamaEmbeddingResponse(float[] Embedding);
    private sealed record OllamaGenerateResponse(string Response);
    private sealed record Hit(Guid Id, string DocumentName, string Collection, string Content, double Score);

    private static readonly IReadOnlyDictionary<string, BotDefinition> Bots = new Dictionary<string, BotDefinition>(StringComparer.OrdinalIgnoreCase)
    {
        ["student"] = new("student", [SmartSchoolRoles.Student, SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin], ["learning","academic","policy"], "You are a study assistant. Explain clearly, teach rather than simply giving assessed-work answers, and use only retrieved school knowledge."),
        ["teacher"] = new("teacher", [SmartSchoolRoles.Teacher, SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin], ["learning","academic","teacher","policy"], "You assist teachers with lesson material, class operations and school policy using retrieved knowledge."),
        ["parent"] = new("parent", [SmartSchoolRoles.Parent, SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin], ["parent","policy","fees","academic"], "You assist parents with school procedures, fees and learning information. Do not expose information about unrelated students."),
        ["admissions"] = new("admissions", [SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin, SmartSchoolRoles.AdmissionOfficer], ["admissions","fees","policy"], "You are the admissions assistant. Answer from approved admissions, fee and policy knowledge."),
        ["admin"] = new("admin", [SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin, SmartSchoolRoles.Principal], ["operations","policy","academic","fees","hr"], "You assist school administrators with operations and policy using authorized retrieved knowledge.")
    };

    public static IEndpointRouteBuilder MapRagChatbotEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var rag = endpoints.MapGroup("/api/rag").WithTags("AI - RAG").RequireAuthorization();
        rag.MapPost("/documents/index", IndexAsync).RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantAdmin);
        endpoints.MapPost("/api/chatbots/{bot}/ask", AskAsync).WithTags("AI - Chatbots").RequireAuthorization();
        return endpoints;
    }

    private static async Task<IResult> IndexAsync(IndexRequest request, ITenantScope scope, IDbConnectionFactory db,
        IHttpClientFactory clients, IConfiguration config, IIntegrationEventPublisher events, CancellationToken ct)
    {
        var tenantId = ResolveTenantForWrite(scope, request.TenantId);
        if (string.IsNullOrWhiteSpace(request.Content) || string.IsNullOrWhiteSpace(request.Collection))
            return Results.BadRequest(new { message = "Collection and content are required." });
        var vector = await EmbedAsync(request.Content, clients, config, ct);
        var vectorLiteral = VectorLiteral(vector);
        const string sql = """
            INSERT INTO ai_core.rag_knowledge_chunk(id, tenant_id, collection, document_name, content, embedding, created_at, is_active)
            VALUES (@Id,@TenantId,@Collection,@DocumentName,@Content,CAST(@Vector AS vector),CURRENT_TIMESTAMP,TRUE);
            """;
        var id = Guid.NewGuid();
        await using var connection = await db.OpenConnectionAsync(ct);
        await connection.ExecuteAsync(new CommandDefinition(sql, new { Id=id, TenantId=tenantId, request.Collection, request.DocumentName, request.Content, Vector=vectorLiteral }, cancellationToken:ct));
        await events.PublishAsync(KafkaTopics.RagDocumentIngestionRequested, new { tenantId, id, request.Collection, request.DocumentName }, ct);
        return Results.Created($"/api/rag/documents/{id}", new { id, tenantId, request.Collection, request.DocumentName });
    }

    private static async Task<IResult> AskAsync(string bot, AskRequest request, ClaimsPrincipal user, ITenantScope scope,
        IDbConnectionFactory db, IHttpClientFactory clients, IConfiguration config, IDistributedCache cache,
        IIntegrationEventPublisher events, CancellationToken ct)
    {
        if (!Bots.TryGetValue(bot, out var definition)) return Results.NotFound(new { message = "Unknown chatbot." });
        if (!definition.Roles.Any(user.IsInRole)) return Results.Forbid();
        if (string.IsNullOrWhiteSpace(request.Question)) return Results.BadRequest(new { message = "Question is required." });

        var tenantId = scope.IsSuperAdmin ? request.TenantId : scope.Resolve(request.TenantId);
        if (!tenantId.HasValue) return Results.BadRequest(new { message = "SuperAdmin must select a tenant for tenant knowledge retrieval." });
        var cacheKey = $"rag:{tenantId}:{definition.Name}:{Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(request.Question.Trim())))}";
        var cached = await cache.GetStringAsync(cacheKey, ct);
        if (cached is not null) return Results.Content(cached, "application/json");

        var vector = await EmbedAsync(request.Question, clients, config, ct);
        var collections = definition.Collections.ToArray();
        const string sql = """
            SELECT id AS "Id", document_name AS "DocumentName", collection AS "Collection", content AS "Content",
                   1 - (embedding <=> CAST(@Vector AS vector)) AS "Score"
            FROM ai_core.rag_knowledge_chunk
            WHERE tenant_id=@TenantId AND is_active=TRUE AND collection = ANY(@Collections)
            ORDER BY embedding <=> CAST(@Vector AS vector)
            LIMIT @TopK;
            """;
        await using var connection = await db.OpenConnectionAsync(ct);
        var hits=(await connection.QueryAsync<Hit>(new CommandDefinition(sql,new { TenantId=tenantId.Value, Collections=collections, Vector=VectorLiteral(vector), TopK=config.GetValue("AI:Ollama:TopK",5)},cancellationToken:ct))).ToArray();
        var context=string.Join("\n\n",hits.Select((h,i)=>$"[{i+1}] {h.DocumentName} ({h.Collection})\n{h.Content}"));
        var prompt=$"{definition.SystemPrompt}\nIf the context does not support the answer, say that the school knowledge base does not contain enough information. Cite [1], [2] where used.\n\nCONTEXT:\n{context}\n\nQUESTION:\n{request.Question}";
        var (answer,model)=await GenerateAsync(prompt,clients,config,ct);
        var response=new AskResponse(definition.Name,answer,model,hits.Select(h=>new Citation(h.Id,h.DocumentName,h.Collection,h.Score)).ToArray());
        var json=JsonSerializer.Serialize(response);
        await cache.SetStringAsync(cacheKey,json,new DistributedCacheEntryOptions{AbsoluteExpirationRelativeToNow=TimeSpan.FromMinutes(5)},ct);
        await events.PublishAsync(KafkaTopics.ChatbotQuestionAsked,new { tenantId, bot=definition.Name, userId=scope.UserId, citationCount=hits.Length },ct);
        return Results.Content(json,"application/json");
    }

    private static Guid ResolveTenantForWrite(ITenantScope scope, Guid? requested) =>
        scope.IsSuperAdmin ? requested ?? throw new BadHttpRequestException("SuperAdmin must select a tenant.") : scope.Resolve(requested)!.Value;

    private static async Task<float[]> EmbedAsync(string text,IHttpClientFactory clients,IConfiguration config,CancellationToken ct)
    {
        var http=clients.CreateClient(); http.BaseAddress=new Uri((config["AI:Ollama:BaseUrl"]??"http://host.docker.internal:11434").TrimEnd('/')+"/");
        var response=await http.PostAsJsonAsync("api/embeddings",new { model=config["AI:Ollama:EmbeddingModel"]??"nomic-embed-text", prompt=text },ct);
        response.EnsureSuccessStatusCode(); var result=await response.Content.ReadFromJsonAsync<OllamaEmbeddingResponse>(cancellationToken:ct);
        return result?.Embedding is { Length: > 0 } v ? v : throw new InvalidOperationException("Ollama returned no embedding.");
    }
    private static async Task<(string Answer,string Model)> GenerateAsync(string prompt,IHttpClientFactory clients,IConfiguration config,CancellationToken ct)
    {
        var model=config["AI:Ollama:ChatModel"]??"llama3.2"; var http=clients.CreateClient(); http.BaseAddress=new Uri((config["AI:Ollama:BaseUrl"]??"http://host.docker.internal:11434").TrimEnd('/')+"/");
        var response=await http.PostAsJsonAsync("api/generate",new { model,prompt,stream=false },ct); response.EnsureSuccessStatusCode();
        var result=await response.Content.ReadFromJsonAsync<OllamaGenerateResponse>(cancellationToken:ct); return (result?.Response??"No response generated.",model);
    }
    private static string VectorLiteral(IEnumerable<float> values)=>"["+string.Join(",",values.Select(x=>x.ToString(CultureInfo.InvariantCulture)))+"]";
    private sealed record BotDefinition(string Name,string[] Roles,string[] Collections,string SystemPrompt);
}
