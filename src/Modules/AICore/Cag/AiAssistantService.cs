using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Dapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Rag;

namespace SmartSchool.Modules.AICore.Cag;

/// <summary>
/// CAG-first AI pipeline. A bounded, authorized knowledge context is cached in Redis and reused
/// across questions. pgvector retrieval is used only when the authorized knowledge set is too
/// large for the configured context window or the CAG context cannot be built.
/// </summary>
internal sealed class AiAssistantService(
    IDbConnectionFactory connectionFactory,
    IDistributedCache cache,
    IOllamaClient ollama,
    LangChainRagWorkflow workflow,
    IOptions<AiAssistantOptions> options) : IAiAssistantService
{
    private sealed record KnowledgeRow(Guid Id, string DocumentName, string Collection, string Content);
    private sealed record RetrievalRow(Guid Id, string DocumentName, string Collection, string Content, double Score);

    private readonly AiAssistantOptions _options = options.Value;

    /// <summary>Answers a question using cached authorized context with RAG fallback.</summary>
    public async Task<AiAssistantResponse> AskAsync(AiAssistantRequest request, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Question);

        var workflowContext = await workflow.ExecuteAsync(
            new RagWorkflowContext(
                request.TenantId,
                request.SchoolId,
                request.UserId,
                request.Assistant,
                request.Question,
                request.Collections,
                request.SystemPrompt),
            cancellationToken);

        if (workflowContext.IsRejected)
        {
            return new AiAssistantResponse(
                request.Assistant,
                workflowContext.Answer ?? "The request was rejected by the RAG guardrail.",
                "none",
                "Guardrail",
                Array.Empty<AiCitation>());
        }

        request = request with { SystemPrompt = workflowContext.SystemPrompt };

        var context = await GetOrBuildContextAsync(request, cancellationToken);
        var strategy = "CAG";

        if (!context.FitsContextWindow || string.IsNullOrWhiteSpace(context.Context))
        {
            context = await RetrieveContextAsync(request, cancellationToken);
            strategy = "RAG-Fallback";
        }

        var prompt = BuildPrompt(request, context.Context);
        var (answer, model) = await ollama.GenerateAsync(prompt, cancellationToken);
        return new AiAssistantResponse(request.Assistant, answer, model, strategy, context.Citations);
    }

    /// <summary>Invalidates a tenant knowledge collection without performing Redis key scans.</summary>
    public async Task InvalidateKnowledgeAsync(Guid tenantId, string collection, CancellationToken cancellationToken)
    {
        var versionKey = GetVersionKey(tenantId, collection);
        await cache.SetStringAsync(
            versionKey,
            Guid.NewGuid().ToString("N"),
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) },
            cancellationToken);
    }

    private async Task<CachedContext> GetOrBuildContextAsync(AiAssistantRequest request, CancellationToken cancellationToken)
    {
        var versions = await GetCollectionVersionsAsync(request.TenantId, request.Collections, cancellationToken);
        var key = BuildContextKey(request, versions);
        var cachedJson = await cache.GetStringAsync(key, cancellationToken);
        if (!string.IsNullOrWhiteSpace(cachedJson))
        {
            var cached = JsonSerializer.Deserialize<CachedContext>(cachedJson);
            if (cached is not null) return cached;
        }

        const string sql = """
            SELECT id AS "Id", document_name AS "DocumentName", collection AS "Collection", content AS "Content"
            FROM ai_core.rag_knowledge_chunk
            WHERE tenant_id = @TenantId
              AND is_active = TRUE
              AND collection = ANY(@Collections)
            ORDER BY created_at DESC
            LIMIT @Limit;
            """;

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var rows = (await connection.QueryAsync<KnowledgeRow>(new CommandDefinition(
            sql,
            new { request.TenantId, Collections = request.Collections.ToArray(), Limit = _options.MaxCachedChunks + 1 },
            cancellationToken: cancellationToken))).ToArray();

        var tooManyRows = rows.Length > _options.MaxCachedChunks;
        var selected = rows.Take(_options.MaxCachedChunks).ToArray();
        var contextText = BuildContextText(selected.Select(row => (row.DocumentName, row.Collection, row.Content)));
        var fits = !tooManyRows && contextText.Length <= _options.MaxContextCharacters;
        if (contextText.Length > _options.MaxContextCharacters) contextText = contextText[.._options.MaxContextCharacters];

        var result = new CachedContext(
            contextText,
            selected.Select(row => new AiCitation(row.Id, row.DocumentName, row.Collection, null)).ToArray(),
            fits,
            DateTimeOffset.UtcNow);

        await cache.SetStringAsync(
            key,
            JsonSerializer.Serialize(result),
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.ContextCacheMinutes) },
            cancellationToken);

        return result;
    }

    private async Task<CachedContext> RetrieveContextAsync(AiAssistantRequest request, CancellationToken cancellationToken)
    {
        var questionHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(request.Question.Trim())));
        var versions = await GetCollectionVersionsAsync(request.TenantId, request.Collections, cancellationToken);
        var versionHash = Hash(string.Join('|', versions));
        var key = $"ai:cag:rag:{request.TenantId:N}:{request.Assistant}:{request.UserId?.ToString("N") ?? "anonymous"}:{versionHash}:{questionHash}";
        var cachedJson = await cache.GetStringAsync(key, cancellationToken);
        if (!string.IsNullOrWhiteSpace(cachedJson))
        {
            var cached = JsonSerializer.Deserialize<CachedContext>(cachedJson);
            if (cached is not null) return cached;
        }

        var embedding = await ollama.EmbedAsync(request.Question, cancellationToken);
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
        var rows = (await connection.QueryAsync<RetrievalRow>(new CommandDefinition(
            sql,
            new
            {
                request.TenantId,
                Collections = request.Collections.ToArray(),
                Vector = ToVectorLiteral(embedding),
                topK = _options.TopK
            },
            cancellationToken: cancellationToken))).ToArray();

        var result = new CachedContext(
            BuildContextText(rows.Select(row => (row.DocumentName, row.Collection, row.Content))),
            rows.Select(row => new AiCitation(row.Id, row.DocumentName, row.Collection, row.Score)).ToArray(),
            true,
            DateTimeOffset.UtcNow);

        await cache.SetStringAsync(
            key,
            JsonSerializer.Serialize(result),
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.RetrievalCacheMinutes) },
            cancellationToken);

        return result;
    }

    private async Task<string[]> GetCollectionVersionsAsync(
        Guid tenantId,
        IReadOnlyCollection<string> collections,
        CancellationToken cancellationToken)
    {
        var versions = new List<string>(collections.Count);
        foreach (var collection in collections.OrderBy(value => value, StringComparer.OrdinalIgnoreCase))
        {
            var key = GetVersionKey(tenantId, collection);
            var version = await cache.GetStringAsync(key, cancellationToken);
            if (string.IsNullOrWhiteSpace(version))
            {
                version = "1";
                await cache.SetStringAsync(
                    key,
                    version,
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) },
                    cancellationToken);
            }
            versions.Add($"{collection}:{version}");
        }
        return versions.ToArray();
    }

    private static string BuildContextKey(AiAssistantRequest request, IReadOnlyCollection<string> versions)
    {
        var actor = request.UserId?.ToString("N") ?? "anonymous";
        var school = request.SchoolId?.ToString("N") ?? "all";
        return $"ai:cag:context:{request.TenantId:N}:{school}:{request.Assistant}:{actor}:{Hash(string.Join('|', versions))}";
    }

    private static string GetVersionKey(Guid tenantId, string collection) =>
        $"ai:cag:version:{tenantId:N}:{collection.Trim().ToLowerInvariant()}";

    private static string BuildContextText(IEnumerable<(string DocumentName, string Collection, string Content)> rows) =>
        string.Join("\n\n", rows.Select((row, index) => $"[{index + 1}] {row.DocumentName} ({row.Collection})\n{row.Content}"));

    private static string BuildPrompt(AiAssistantRequest request, string context) => $"""
        {request.SystemPrompt}

        SECURITY AND GROUNDING RULES:
        - Use only the authorized context below for school-specific facts.
        - Never expose another tenant's or unrelated user's information.
        - If the context does not support an answer, say that verified school knowledge is insufficient.
        - Cite context items as [1], [2], and so on when they support the answer.

        AUTHORIZED CONTEXT:
        {context}

        QUESTION:
        {request.Question}
        """;

    private static string ToVectorLiteral(IEnumerable<float> values) =>
        "[" + string.Join(',', values.Select(value => value.ToString(CultureInfo.InvariantCulture))) + "]";

    private static string Hash(string value) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value)));
}
