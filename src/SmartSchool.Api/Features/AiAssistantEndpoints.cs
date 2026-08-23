using SmartSchool.SharedKernel;
using System.Net.Http.Json;
using Dapper;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Api.Features;

public static class AiAssistantEndpoints
{
    public sealed record AskRequest(Guid TenantId, string Question, string? Actor, Guid? SchoolId);
    public sealed record AskResponse(string Answer, IReadOnlyCollection<Citation> Citations, string Model);
    public sealed record Citation(Guid Id, string Name, double Score);
    private sealed record OllamaEmbeddingResponse(float[] Embedding);
    private sealed record OllamaGenerateResponse(string Response);
    private sealed record Chunk(Guid Id, string Name, string Content, double Score);

    public static IEndpointRouteBuilder MapAiAssistantEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/ai/assistant/ask", AskAsync)
            .WithTags("AI - Assistant")
            .RequireAuthorization();
        return endpoints;
    }

    private static async Task<IResult> AskAsync(
        AskRequest request, IHttpClientFactory clients, IDbConnectionFactory db,
        IConfiguration configuration, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
            return Results.BadRequest(new { message = "Question is required." });

        var baseUrl = configuration["AI:Ollama:BaseUrl"] ?? "http://host.docker.internal:11434";
        var chatModel = configuration["AI:Ollama:ChatModel"] ?? "llama3.2";
        var embeddingModel = configuration["AI:Ollama:EmbeddingModel"] ?? "nomic-embed-text";
        var http = clients.CreateClient();
        http.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");

        // Ollama embeddings.
        var embeddingHttp = await http.PostAsJsonAsync("api/embeddings",
            new { model = embeddingModel, prompt = request.Question }, ct);
        embeddingHttp.EnsureSuccessStatusCode();
        var embedding = await embeddingHttp.Content.ReadFromJsonAsync<OllamaEmbeddingResponse>(cancellationToken: ct);
        if (embedding?.Embedding is null || embedding.Embedding.Length == 0)
            return Results.Problem("Embedding model returned no vector.");

        // pgvector: migration v55 adds embedding/content columns to ai.knowledge_chunk.
        var vectorLiteral = "[" + string.Join(",", embedding.Embedding.Select(x => x.ToString(System.Globalization.CultureInfo.InvariantCulture))) + "]";
        const string sql = """
            SELECT id AS "Id", document_name AS "Name", content AS "Content",
                   1 - (embedding <=> CAST(@Vector AS vector)) AS "Score"
            FROM ai_core.rag_knowledge_chunk
            WHERE tenant_id=@TenantId AND is_active=true AND embedding IS NOT NULL
            ORDER BY embedding <=> CAST(@Vector AS vector)
            LIMIT 5;
            """;
        await using var connection = await db.OpenConnectionAsync(ct);
        var chunks = (await connection.QueryAsync<Chunk>(
            new CommandDefinition(sql, new { request.TenantId, Vector = vectorLiteral }, cancellationToken: ct))).ToArray();

        var context = string.Join("\n\n", chunks.Select((x, i) => $"[{i + 1}] {x.Name}\n{x.Content}"));
        var system = $"You are SmartSchool assistant for actor {request.Actor ?? "User"}. Answer only from authorized tenant context. If context is insufficient, say so. Cite sources as [1], [2].";
        var prompt = $"{system}\n\nCONTEXT:\n{context}\n\nQUESTION:\n{request.Question}";

        var generationHttp = await http.PostAsJsonAsync("api/generate",
            new { model = chatModel, prompt, stream = false }, ct);
        generationHttp.EnsureSuccessStatusCode();
        var generated = await generationHttp.Content.ReadFromJsonAsync<OllamaGenerateResponse>(cancellationToken: ct);

        return Results.Ok(new AskResponse(
            generated?.Response ?? "No response generated.",
            chunks.Select(x => new Citation(x.Id, x.Name, x.Score)).ToArray(),
            chatModel));
    }
}
