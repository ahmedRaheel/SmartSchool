using System.Globalization;
using System.Text.RegularExpressions;
using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Cag;
using SmartSchool.SharedKernel.Constants;
using UglyToad.PdfPig;

namespace SmartSchool.Modules.AICore.Features.KnowledgeDocument;

public static class UploadKnowledgePdf
{
    private const long MaxPdfSize = 25 * 1024 * 1024;
    private const int ChunkSize = 1200;

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/aicore/knowledge/pdf", UploadAsync)
            .WithTags("AICore Knowledge")
            .WithName("UploadKnowledgePdf")
            .RequireAuthorization(SmartSchoolPolicies.AiKnowledgeContribution)
            .DisableAntiforgery();
    }

    private static async Task<IResult> UploadAsync(
        IFormFile file,
        Guid collectionId,
        Guid? tenantId,
        Guid? campusId,
        Guid? academicSystemId,
        ITenantScope tenantScope,
        IDbConnectionFactory connectionFactory,
        IOllamaClient ollamaClient,
        IAiAssistantService assistantService,
        CancellationToken cancellationToken)
    {
        var resolvedTenantId = tenantScope.Resolve(tenantId);
        if (!resolvedTenantId.HasValue)
        {
            return Results.BadRequest(new { message = "A tenant is required." });
        }

        if (file.Length is 0 or > MaxPdfSize)
        {
            return Results.BadRequest(new { message = "PDF must be between 1 byte and 25 MB." });
        }

        if (!string.Equals(Path.GetExtension(file.FileName), ".pdf", StringComparison.OrdinalIgnoreCase))
        {
            return Results.BadRequest(new { message = "Only PDF files are supported." });
        }

        await using var stream = file.OpenReadStream();
        using var pdf = PdfDocument.Open(stream);

        var pages = pdf.GetPages()
            .Select(page => Regex.Replace(page.Text ?? string.Empty, @"\s+", " ").Trim())
            .Where(text => text.Length > 0)
            .ToArray();

        if (pages.Length == 0)
        {
            return Results.BadRequest(
                new { message = "No extractable text was found. Scanned/image-only PDFs require OCR before ingestion." });
        }

        var documentId = Guid.NewGuid();
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);

        const string verifyCollectionSql = """
            SELECT code
            FROM ai_core.knowledge_collection
            WHERE tenant_id = @TenantId
              AND knowledge_collection_id = @CollectionId
              AND is_active = true;
            """;

        var collectionCode = await connection.QuerySingleOrDefaultAsync<string>(
            new CommandDefinition(
                verifyCollectionSql,
                new
                {
                    TenantId = resolvedTenantId.Value,
                    CollectionId = collectionId
                },
                cancellationToken: cancellationToken));

        if (string.IsNullOrWhiteSpace(collectionCode))
        {
            return Results.BadRequest(new { message = "Knowledge collection does not belong to this tenant." });
        }

        const string insertDocumentSql = """
            INSERT INTO ai_core.knowledge_document
            (
                knowledge_document_id,
                knowledge_collection_id,
                tenant_id,
                campus_id,
                academic_system_id,
                title,
                document_type,
                source_url,
                metadata,
                status,
                is_active,
                created_at,
                row_version
            )
            VALUES
            (
                @DocumentId,
                @CollectionId,
                @TenantId,
                @CampusId,
                @AcademicSystemId,
                @Title,
                'PDF',
                NULL,
                CAST(@Metadata AS jsonb),
                'INDEXED',
                true,
                CURRENT_TIMESTAMP,
                gen_random_bytes(8)
            );
            """;

        await connection.ExecuteAsync(
            new CommandDefinition(
                insertDocumentSql,
                new
                {
                    DocumentId = documentId,
                    CollectionId = collectionId,
                    TenantId = resolvedTenantId.Value,
                    CampusId = campusId,
                    AcademicSystemId = academicSystemId,
                    Title = Path.GetFileName(file.FileName),
                    Metadata = "{\"source\":\"upload\"}"
                },
                cancellationToken: cancellationToken));

        var chunks = Chunk(pages, ChunkSize).ToArray();

        const string insertChunkSql = """
            INSERT INTO ai_core.rag_knowledge_chunk
            (
                id,
                tenant_id,
                collection,
                document_name,
                content,
                embedding,
                created_at,
                is_active
            )
            VALUES
            (
                @Id,
                @TenantId,
                @Collection,
                @DocumentName,
                @Content,
                CAST(@Embedding AS vector),
                CURRENT_TIMESTAMP,
                true
            );
            """;

        foreach (var batch in chunks.Chunk(32))
        {
            var embeddings = await ollamaClient.EmbedBatchAsync(batch, cancellationToken);

            var rows = batch.Select((content, index) => new
            {
                Id = Guid.NewGuid(),
                TenantId = resolvedTenantId.Value,
                Collection = collectionCode.Trim().ToLowerInvariant(),
                DocumentName = Path.GetFileName(file.FileName),
                Content = content,
                Embedding = ToVectorLiteral(embeddings[index])
            }).ToArray();

            await connection.ExecuteAsync(
                new CommandDefinition(insertChunkSql, rows, cancellationToken: cancellationToken));
        }

        await assistantService.InvalidateKnowledgeAsync(
            resolvedTenantId.Value,
            collectionCode,
            cancellationToken);

        return Results.Ok(
            new
            {
                knowledgeDocumentId = documentId,
                fileName = file.FileName,
                pages = pages.Length,
                chunks = chunks.Length,
                indexed = true
            });
    }

    private static IEnumerable<string> Chunk(IEnumerable<string> pages, int maxLength)
    {
        foreach (var page in pages)
        {
            for (var index = 0; index < page.Length; index += maxLength)
            {
                yield return page.Substring(index, Math.Min(maxLength, page.Length - index));
            }
        }
    }

    private static string ToVectorLiteral(IEnumerable<float> values)
    {
        return $"[{string.Join(",", values.Select(value => value.ToString(CultureInfo.InvariantCulture)))}]";
    }
}
