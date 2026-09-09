using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Features.Document;

public static class GetDocumentPage
{
    public sealed record Query(Guid TenantId, Guid? CampusId, string? OwnerType, Guid? OwnerId, int Page, int PageSize)
        : IRequest<Result<Response>>;

    public sealed record Item(
        Guid DocumentId,
        string DocumentNumber,
        string DocumentTypeCode,
        string DocumentTypeName,
        string OwnerType,
        Guid OwnerId,
        string FileName,
        string? Title,
        DateTimeOffset CreatedAt);

    public sealed record Response(IReadOnlyCollection<Item> Items, int TotalCount, int Page, int PageSize);

    public interface IGetDocumentPageQuery
    {
        Task<Response> GetAsync(Guid tenantId, Guid? campusId, string? ownerType, Guid? ownerId, int page, int pageSize, CancellationToken cancellationToken);
    }

    internal sealed class GetDocumentPageQuery(IDbConnectionFactory connectionFactory) : IGetDocumentPageQuery
    {
        public async Task<Response> GetAsync(Guid tenantId, Guid? campusId, string? ownerType, Guid? ownerId, int page, int pageSize, CancellationToken cancellationToken)
        {
            const string countSql = """
                SELECT COUNT(*)
                FROM document.document d
                WHERE d.tenant_id = @TenantId
                  AND (@CampusId IS NULL OR d.campus_id = @CampusId)
                  AND (@OwnerType IS NULL OR d.owner_type = @OwnerType)
                  AND (@OwnerId IS NULL OR d.owner_id = @OwnerId)
                  AND d.is_active = TRUE;
                """;
            const string dataSql = """
                SELECT
                    d.document_id AS "DocumentId",
                    d.document_number AS "DocumentNumber",
                    dt.code AS "DocumentTypeCode",
                    dt.name AS "DocumentTypeName",
                    d.owner_type AS "OwnerType",
                    d.owner_id AS "OwnerId",
                    d.original_file_name AS "FileName",
                    d.title AS "Title",
                    d.created_at AS "CreatedAt"
                FROM document.document d
                JOIN document.document_type dt ON dt.document_type_id = d.document_type_id
                WHERE d.tenant_id = @TenantId
                  AND (@CampusId IS NULL OR d.campus_id = @CampusId)
                  AND (@OwnerType IS NULL OR d.owner_type = @OwnerType)
                  AND (@OwnerId IS NULL OR d.owner_id = @OwnerId)
                  AND d.is_active = TRUE
                ORDER BY d.created_at DESC
                OFFSET @Offset LIMIT @PageSize;
                """;

            var parameters = new
            {
                TenantId = tenantId,
                CampusId = campusId,
                OwnerType = ownerType?.ToUpperInvariant(),
                OwnerId = ownerId,
                Offset = (page - 1) * pageSize,
                PageSize = pageSize
            };

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var totalCount = await connection.ExecuteScalarAsync<int>(new CommandDefinition(countSql, parameters, cancellationToken: cancellationToken));
            var items = (await connection.QueryAsync<Item>(new CommandDefinition(dataSql, parameters, cancellationToken: cancellationToken))).AsList();
            return new Response(items, totalCount, page, pageSize);
        }
    }

    public sealed class Handler(IGetDocumentPageQuery query) : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Query request, CancellationToken cancellationToken)
        {
            var response = await query.GetAsync(request.TenantId, request.CampusId, request.OwnerType, request.OwnerId, request.Page, request.PageSize, cancellationToken);
            return Result<Response>.Success(response);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/documents",
                async (string? ownerType, Guid? ownerId, int? page, int? pageSize, ICurrentUser currentUser, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    if (currentUser.TenantId is not Guid tenantId)
                    {
                        return Results.Forbid();
                    }

                    var query = new Query(tenantId, currentUser.BranchId, ownerType, ownerId, Math.Max(1, page ?? 1), Math.Clamp(pageSize ?? 20, 1, 100));
                    var result = await mediator.SendAsync<Query, Result<Response>>(query, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetDocumentPage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
