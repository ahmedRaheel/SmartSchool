using System.Globalization;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Dapper;
using Microsoft.Extensions.Caching.Hybrid;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.AICore;

public sealed record AiKnowledgeCitation(Guid Id, string DocumentName, string Collection, double Score);
public sealed record AiKnowledgeContext(string Content, IReadOnlyCollection<AiKnowledgeCitation> Citations, string Source, string Version);
public sealed record AiKnowledgeRequest(Guid TenantId, Guid? SchoolId, Guid? ActorId, string Assistant, string Question, string[] Collections);

public interface IAiContextService
{
    Task<AiKnowledgeContext> GetAsync(AiKnowledgeRequest request, CancellationToken cancellationToken);
    Task InvalidateTenantAsync(Guid tenantId, CancellationToken cancellationToken);
}

/// <summary>
/// CAG-first knowledge service. Stable authorized context is cached in HybridCache/Redis.
/// pgvector is used only when the cached context is unavailable or does not cover the question.
/// </summary>
public sealed class AiContextService(
    HybridCache cache,
    IDbConnectionFactory connectionFactory,
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration) : IAiContextService
{
    private sealed record ContextRow(Guid Id, string DocumentName, string Collection, string Content, double Score);
    private sealed record OllamaEmbeddingResponse(float[][] Embeddings);

    public async Task<AiKnowledgeContext> GetAsync(AiKnowledgeRequest request, CancellationToken cancellationToken)
    {
        var version = await GetKnowledgeVersionAsync(request.TenantId, cancellationToken);
        var scopeKey = BuildScopeKey(request, version);

        var cachedContext = await cache.GetOrCreateAsync(
            scopeKey,
            async token => await BuildCachedContextAsync(request, version, token),
            new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(configuration.GetValue("AI:CAG:ContextTtlMinutes", 30)),
                LocalCacheExpiration = TimeSpan.FromMinutes(5)
            },
            cancellationToken: cancellationToken);

        if (cachedContext.Citations.Count > 0 && IsLikelyCovered(request.Question, cachedContext.Content))
        {
            return cachedContext with { Source = "cag" };
        }

        return await RetrieveWithRagAsync(request, version, cancellationToken);
    }

    public async Task InvalidateTenantAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        var versionKey = $"ai:cag:version:{tenantId:N}";
        await cache.SetAsync(versionKey, Guid.NewGuid().ToString("N"), cancellationToken: cancellationToken);
    }

    private async Task<string> GetKnowledgeVersionAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        var key = $"ai:cag:version:{tenantId:N}";
        return await cache.GetOrCreateAsync(
            key,
            _ => ValueTask.FromResult(Guid.NewGuid().ToString("N")),
            new HybridCacheEntryOptions { Expiration = TimeSpan.FromDays(30) },
            cancellationToken: cancellationToken);
    }

    private async ValueTask<AiKnowledgeContext> BuildCachedContextAsync(
        AiKnowledgeRequest request,
        string version,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT id AS "Id", document_name AS "DocumentName", collection AS "Collection", content AS "Content", 1.0 AS "Score"
            FROM ai_core.rag_knowledge_chunk
            WHERE tenant_id = @TenantId
              AND is_active = TRUE
              AND collection = ANY(@Collections)
            ORDER BY created_at DESC
            LIMIT @Limit;
            """;

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var rows = (await connection.QueryAsync<ContextRow>(new CommandDefinition(
            sql,
            new
            {
                request.TenantId,
                request.Collections,
                Limit = configuration.GetValue("AI:CAG:MaxCachedChunks", 20)
            },
            cancellationToken: cancellationToken))).ToArray();

        return BuildContext(rows, "cag", version);
    }

    private async Task<AiKnowledgeContext> RetrieveWithRagAsync(
        AiKnowledgeRequest request,
        string version,
        CancellationToken cancellationToken)
    {
        var embedding = await EmbedAsync(request.Question, cancellationToken);
        const string sql = """
            SELECT id AS "Id", document_name AS "DocumentName", collection AS "Collection", content AS "Content",
                   1 - (embedding <=> CAST(@Vector AS vector)) AS "Score"
            FROM ai_core.rag_knowledge_chunk
            WHERE tenant_id = @TenantId
              AND is_active = TRUE
              AND collection = ANY(@Collections)
            ORDER BY embedding <=> CAST(@Vector AS vector)
            LIMIT @TopK;
            """;

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var rows = (await connection.QueryAsync<ContextRow>(new CommandDefinition(
            sql,
            new
            {
                request.TenantId,
                request.Collections,
                Vector = VectorLiteral(embedding),
                TopK = configuration.GetValue("AI:Ollama:TopK", 5)
            },
            cancellationToken: cancellationToken))).ToArray();

        return BuildContext(rows, "rag", version);
    }

    private async Task<float[]> EmbedAsync(string text, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient();
        client.BaseAddress = new Uri((configuration["AI:Ollama:BaseUrl"] ?? throw new InvalidOperationException("AI:Ollama:BaseUrl configuration is required.")).TrimEnd('/') + "/");
        var response = await client.PostAsJsonAsync(
            "api/embed",
            new { model = configuration["AI:Ollama:EmbeddingModel"] ?? "nomic-embed-text", input = text },
            cancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<OllamaEmbeddingResponse>(cancellationToken: cancellationToken);
        return result?.Embeddings is { Length: > 0 } && result.Embeddings[0].Length > 0
            ? result.Embeddings[0]
            : throw new InvalidOperationException("Ollama returned no embedding.");
    }

    private static AiKnowledgeContext BuildContext(IEnumerable<ContextRow> rows, string source, string version)
    {
        var materialized = rows.ToArray();
        var content = string.Join("\n\n", materialized.Select((row, index) =>
            $"[{index + 1}] {row.DocumentName} ({row.Collection})\n{row.Content}"));
        var citations = materialized.Select(row =>
            new AiKnowledgeCitation(row.Id, row.DocumentName, row.Collection, row.Score)).ToArray();
        return new AiKnowledgeContext(content, citations, source, version);
    }

    private static string BuildScopeKey(AiKnowledgeRequest request, string version)
    {
        var collections = string.Join('-', request.Collections.OrderBy(value => value, StringComparer.OrdinalIgnoreCase));
        return $"ai:cag:context:{request.TenantId:N}:{request.SchoolId?.ToString("N") ?? "all"}:{request.ActorId?.ToString("N") ?? "shared"}:{request.Assistant}:{collections}:{version}";
    }

    private static bool IsLikelyCovered(string question, string context)
    {
        if (string.IsNullOrWhiteSpace(context)) return false;
        var terms = question.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(term => term.Length >= 4)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(12)
            .ToArray();
        if (terms.Length == 0) return true;
        return terms.Any(term => context.Contains(term, StringComparison.OrdinalIgnoreCase));
    }

    private static string VectorLiteral(IEnumerable<float> values) =>
        "[" + string.Join(",", values.Select(value => value.ToString(CultureInfo.InvariantCulture))) + "]";
}
