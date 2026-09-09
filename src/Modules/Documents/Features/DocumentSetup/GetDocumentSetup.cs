using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Features.DocumentSetup;

public static class GetDocumentSetup
{
    public sealed record Query(
        Guid TenantId,
        Guid? CampusId) : IRequest<Result<Response>>;

    public sealed record Lookup(
        Guid Id,
        string Code,
        string Name,
        string? OwnerType);

    public sealed record Requirement(
        Guid Id,
        string UserRole,
        bool IsMandatory,
        Guid RequiredDocumentTypeId,
        string RequiredDocumentTypeName);

    public sealed record Response(
        IReadOnlyCollection<Lookup> DocumentTypes,
        IReadOnlyCollection<Lookup> RequiredDocumentTypes,
        IReadOnlyCollection<Requirement> RequiredDocuments);

    public interface IGetDocumentSetupQuery
    {
        Task<Response> GetAsync(
            Guid tenantId,
            Guid? campusId,
            CancellationToken cancellationToken);
    }

    internal sealed class GetDocumentSetupQuery(
        IDbConnectionFactory connectionFactory) : IGetDocumentSetupQuery
    {
        public async Task<Response> GetAsync(
            Guid tenantId,
            Guid? campusId,
            CancellationToken cancellationToken)
        {
            const string documentTypesSql = """
                SELECT
                    document_type_id AS "Id",
                    code AS "Code",
                    name AS "Name",
                    owner_type AS "OwnerType"
                FROM document.document_type
                WHERE tenant_id = @TenantId
                  AND (campus_id IS NULL OR campus_id = @CampusId)
                  AND is_active = TRUE
                ORDER BY name;
                """;

            const string requiredTypesSql = """
                SELECT
                    required_document_type_id AS "Id",
                    code AS "Code",
                    name AS "Name",
                    NULL::text AS "OwnerType"
                FROM document.required_document_type
                WHERE tenant_id = @TenantId
                  AND (campus_id IS NULL OR campus_id = @CampusId)
                  AND is_active = TRUE
                ORDER BY name;
                """;

            const string requirementsSql = """
                SELECT
                    required_document_id AS "Id",
                    required_document.user_role AS "UserRole",
                    required_document.is_mandatory AS "IsMandatory",
                    required_document.required_document_type_id AS "RequiredDocumentTypeId",
                    required_document_type.name AS "RequiredDocumentTypeName"
                FROM document.required_document
                JOIN document.required_document_type
                    ON document.required_document_type.required_document_type_id =
                       document.required_document.required_document_type_id
                WHERE document.required_document.tenant_id = @TenantId
                  AND (
                      document.required_document.campus_id IS NULL
                      OR document.required_document.campus_id = @CampusId)
                  AND document.required_document.is_active = TRUE
                ORDER BY
                    document.required_document.user_role,
                    document.required_document_type.name;
                """;

            var parameters = new
            {
                TenantId = tenantId,
                CampusId = campusId
            };

            await using var connection =
                await connectionFactory.OpenConnectionAsync(cancellationToken);

            var documentTypes = (await connection.QueryAsync<Lookup>(
                new CommandDefinition(
                    documentTypesSql,
                    parameters,
                    cancellationToken: cancellationToken))).AsList();

            var requiredDocumentTypes = (await connection.QueryAsync<Lookup>(
                new CommandDefinition(
                    requiredTypesSql,
                    parameters,
                    cancellationToken: cancellationToken))).AsList();

            var requiredDocuments = (await connection.QueryAsync<Requirement>(
                new CommandDefinition(
                    requirementsSql,
                    parameters,
                    cancellationToken: cancellationToken))).AsList();

            return new Response(
                documentTypes,
                requiredDocumentTypes,
                requiredDocuments);
        }
    }

    public sealed class Handler(
        IGetDocumentSetupQuery query) : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var response = await query.GetAsync(
                request.TenantId,
                request.CampusId,
                cancellationToken);

            return Result<Response>.Success(response);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/documents/setup",
                async (
                    ICurrentUser currentUser,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    if (currentUser.TenantId is not Guid tenantId)
                    {
                        return Results.Forbid();
                    }

                    var query = new Query(
                        tenantId,
                        currentUser.BranchId);

                    var result = await mediator.SendAsync<Query, Result<Response>>(
                        query,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("GetDocumentSetup")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
