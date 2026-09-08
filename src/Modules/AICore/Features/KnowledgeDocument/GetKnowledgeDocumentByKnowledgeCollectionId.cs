using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AICore.Features.KnowledgeDocument;

public static class GetKnowledgeDocumentByKnowledgeCollectionId
{
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

    public sealed record Query(Guid TenantId, Guid ParentId)
        : IRequest<Result<IReadOnlyCollection<Response>>>;

    public interface IGetKnowledgeDocumentByKnowledgeCollectionIdQuery
    {
        Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid parentId,
            CancellationToken cancellationToken);
    }

    internal sealed class GetKnowledgeDocumentByKnowledgeCollectionIdQuery(IDbConnectionFactory connectionFactory)
        : IGetKnowledgeDocumentByKnowledgeCollectionIdQuery
    {
        public async Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid parentId,
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
                      AND entity.knowledge_collection_id = @ParentId
                      AND entity.is_active = TRUE;
                    """;

            await using var connection =
                await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

            var items = await connection.QueryAsync<Response>(
                new CommandDefinition(
                    sql,
                    new { TenantId = tenantId, ParentId = parentId },
                    cancellationToken: cancellationToken)).ConfigureAwait(false);

            return items.AsList();
        }
    }

    public sealed class Handler(IGetKnowledgeDocumentByKnowledgeCollectionIdQuery query)
        : IRequestHandler<Query, Result<IReadOnlyCollection<Response>>>
    {
        public async Task<Result<IReadOnlyCollection<Response>>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var items = await query.GetAsync(
                request.TenantId,
                request.ParentId,
                cancellationToken).ConfigureAwait(false);

            return Result<IReadOnlyCollection<Response>>.Success(items);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/aicore/knowledge-document/by-knowledge-collection/{parentId:guid}",
                async (Guid parentId, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Query, Result<IReadOnlyCollection<Response>>>(
                        new Query(tenantId, parentId),
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetKnowledgeDocumentByKnowledgeCollectionId")
            .WithTags("AICore")
            .RequireAuthorization();

        return endpoints;
    }
}
