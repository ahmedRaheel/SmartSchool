using SmartSchool.Application.Persistence;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AICore.Features.KnowledgeDocument;

public static class GetKnowledgeDocumentById
{
    /// <summary>
    /// Represents the response returned by this KnowledgeDocumentEntity feature.
    /// </summary>
    /// <param name="TenantId">The owning tenant identifier.</param>
    /// <param name="Id">The entity identifier.</param>
    /// <param name="Code">The business code.</param>
    /// <param name="Name">The display name.</param>
    public sealed record Response(
    Guid TenantId,
    Guid Id,
    string Code,
    string Name,
    string? MetadataJson,
    Guid? AcademicSystemId,
    string? AcademicSystemCode,
    string? AcademicSystemName,
    Guid? CampusId,
    string? CampusCode,
    string? CampusName,
    Guid KnowledgeCollectionId,
    string? KnowledgeCollectionCode,
    string? KnowledgeCollectionName);

    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<Response>>;

    public interface IGetKnowledgeDocumentByIdQuery
    {
        Task<Response?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken);

    }

    internal sealed class GetKnowledgeDocumentByIdQuery(
        IDbConnectionFactory connectionFactory) : IGetKnowledgeDocumentByIdQuery
    {
        public async Task<Response?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken)
            {
                const string sql = """
                    SELECT
                        entity.tenant_id AS "TenantId",
                        entity.knowledge_document_id AS "Id",
                        entity.code AS "Code",
                        entity.name AS "Name",
                        entity.metadata_json AS "MetadataJson",
                        p1.academic_system_id AS "AcademicSystemId",
                        p1.code AS "AcademicSystemCode",
                        p1.name AS "AcademicSystemName",
                        p2.campus_id AS "CampusId",
                        p2.code AS "CampusCode",
                        p2.name AS "CampusName",
                        p3.knowledge_collection_id AS "KnowledgeCollectionId",
                        p3.code AS "KnowledgeCollectionCode",
                        p3.name AS "KnowledgeCollectionName"
                    FROM ai_core.knowledge_document AS entity
                    LEFT JOIN academic.academic_system AS p1
                        ON p1.academic_system_id = entity.academic_system_id
                    LEFT JOIN org.campus AS p2
                        ON p2.campus_id = entity.campus_id
                    LEFT JOIN ai_core.knowledge_collection AS p3
                        ON p3.knowledge_collection_id = entity.knowledge_collection_id
                    WHERE entity.tenant_id = @TenantId
                      AND entity.knowledge_document_id = @Id
                      AND entity.is_active = TRUE;
                    """;

                await using var connection =
                    await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

                return await connection.QuerySingleOrDefaultAsync<Response>(
                    new CommandDefinition(
                        sql,
                        new { TenantId = tenantId, Id = id },
                        cancellationToken: cancellationToken)).ConfigureAwait(false);
            }
    }

    public sealed class Handler(IGetKnowledgeDocumentByIdQuery query)
        : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await query.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<Response>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(KnowledgeDocumentEntity))));
            }
            return Result<Response>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "knowledge-document"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetKnowledgeDocumentById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
