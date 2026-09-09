using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Documents.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Features.Document;

public static class GetDocumentById
{
    public sealed record Query(Guid DocumentId, Guid TenantId, Guid? CampusId) : IRequest<Result<Response>>;

    public sealed record Response(
        Guid DocumentId,
        string DocumentNumber,
        Guid DocumentTypeId,
        string DocumentTypeCode,
        string DocumentTypeName,
        string OwnerType,
        Guid OwnerId,
        Guid? RequiredDocumentTypeId,
        string? RequiredDocumentTypeName,
        string FileName,
        string MimeType,
        long SizeBytes,
        string? Title,
        bool IsConfidential,
        DateTimeOffset CreatedAt);

    public interface IGetDocumentByIdQuery
    {
        Task<Response?> GetAsync(Guid documentId, Guid tenantId, Guid? campusId, CancellationToken cancellationToken);
    }

    internal sealed class GetDocumentByIdQuery(IDbConnectionFactory connectionFactory) : IGetDocumentByIdQuery
    {
        public async Task<Response?> GetAsync(Guid documentId, Guid tenantId, Guid? campusId, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    d.document_id AS "DocumentId",
                    d.document_number AS "DocumentNumber",
                    d.document_type_id AS "DocumentTypeId",
                    dt.code AS "DocumentTypeCode",
                    dt.name AS "DocumentTypeName",
                    d.owner_type AS "OwnerType",
                    d.owner_id AS "OwnerId",
                    d.required_document_type_id AS "RequiredDocumentTypeId",
                    rdt.name AS "RequiredDocumentTypeName",
                    d.original_file_name AS "FileName",
                    d.mime_type AS "MimeType",
                    d.size_bytes AS "SizeBytes",
                    d.title AS "Title",
                    d.is_confidential AS "IsConfidential",
                    d.created_at AS "CreatedAt"
                FROM document.document d
                JOIN document.document_type dt ON dt.document_type_id = d.document_type_id
                LEFT JOIN document.required_document_type rdt ON rdt.required_document_type_id = d.required_document_type_id
                WHERE d.document_id = @DocumentId
                  AND d.tenant_id = @TenantId
                  AND (@CampusId IS NULL OR d.campus_id = @CampusId)
                  AND d.is_active = TRUE;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<Response>(new CommandDefinition(
                sql,
                new { DocumentId = documentId, TenantId = tenantId, CampusId = campusId },
                cancellationToken: cancellationToken));
        }
    }

    public sealed class Handler(IGetDocumentByIdQuery query) : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Query request, CancellationToken cancellationToken)
        {
            var response = await query.GetAsync(request.DocumentId, request.TenantId, request.CampusId, cancellationToken);
            return response is null
                ? Result<Response>.Failure(Error.NotFound("Document was not found."))
                : Result<Response>.Success(response);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/documents/{documentId:guid}",
                async (Guid documentId, ICurrentUser currentUser, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    if (currentUser.TenantId is not Guid tenantId)
                    {
                        return Results.Forbid();
                    }

                    var result = await mediator.SendAsync<Query, Result<Response>>(
                        new Query(documentId, tenantId, currentUser.BranchId),
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetDocumentById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
