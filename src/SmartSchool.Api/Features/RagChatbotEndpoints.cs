using System.Security.Claims;
using System.Globalization;
using System.Net.Http.Json;
using Dapper;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Application.Identity;
using SmartSchool.SharedKernel.Constants;
using SmartSchool.Modules.AICore;

namespace SmartSchool.Api.Features;

public static class RagChatbotEndpoints
{
    public sealed record IndexRequest(Guid? TenantId, string Collection, string DocumentName, string Content);
    public sealed record AskRequest(Guid? TenantId, string Question, Guid? SchoolId = null);
    public sealed record Citation(Guid Id, string DocumentName, string Collection, double Score);
    public sealed record AskResponse(string Bot, string Answer, string Model, string ContextSource, string ContextVersion, IReadOnlyCollection<Citation> Citations);
    private sealed record OllamaEmbeddingResponse(float[] Embedding);
    private sealed record OllamaGenerateResponse(string Response);

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
        IHttpClientFactory clients, IConfiguration config, IIntegrationEventPublisher events,
        IAiContextService contextService, CancellationToken ct)
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
        await contextService.InvalidateTenantAsync(tenantId, ct);
        await events.PublishAsync(KafkaTopics.RagDocumentIngestionRequested, new { tenantId, id, request.Collection, request.DocumentName }, ct);
        return Results.Created($"/api/rag/documents/{id}", new { id, tenantId, request.Collection, request.DocumentName });
    }

    private static async Task<IResult> AskAsync(
        string bot,
        AskRequest request,
        ClaimsPrincipal user,
        ITenantScope scope,
        IAiContextService contextService,
        IHttpClientFactory clients,
        IConfiguration config,
        IIntegrationEventPublisher events,
        CancellationToken ct)
    {
        if (!Bots.TryGetValue(bot, out var definition))
            return Results.NotFound(new { message = "Unknown chatbot." });
        if (!definition.Roles.Any(user.IsInRole))
            return Results.Forbid();
        if (string.IsNullOrWhiteSpace(request.Question))
            return Results.BadRequest(new { message = "Question is required." });

        var tenantId = scope.IsSuperAdmin ? request.TenantId : scope.Resolve(request.TenantId);
        if (!tenantId.HasValue)
            return Results.BadRequest(new { message = "SuperAdmin must select a tenant." });

        var knowledge = await contextService.GetAsync(
            new AiKnowledgeRequest(
                tenantId.Value,
                request.SchoolId,
                scope.UserId,
                definition.Name,
                request.Question.Trim(),
                definition.Collections),
            ct);

        var prompt = $"""
{definition.SystemPrompt} The supplied context is authorized for the current tenant and actor.
If it does not support the answer, say that the school knowledge base does not contain enough information.
		
Cite [1], [2] where used. Never reveal another tenant's or unauthorized actor's data.

CONTEXT SOURCE: {knowledge.Source}
CONTEXT:
{knowledge.Content}

QUESTION:
{request.Question}
""";

        var (answer, model) = await GenerateAsync(prompt, clients, config, ct);
        var response = new AskResponse(
            definition.Name,
            answer,
            model,
            knowledge.Source,
            knowledge.Version,
            knowledge.Citations.Select(citation =>
                new Citation(citation.Id, citation.DocumentName, citation.Collection, citation.Score)).ToArray());

        await events.PublishAsync(
            KafkaTopics.ChatbotQuestionAsked,
            new
            {
                tenantId = tenantId.Value,
                bot = definition.Name,
                userId = scope.UserId,
                contextSource = knowledge.Source,
                contextVersion = knowledge.Version,
                citationCount = knowledge.Citations.Count
            },
            ct);

        return Results.Ok(response);
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
