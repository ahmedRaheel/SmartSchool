using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Features.TopicPerformanceInsight;

public static class GetTopicPerformanceInsightByClassPerformanceInsightId
{
    public sealed record Response(
    Guid TenantId,
    Guid Id,
    string Code,
    string Name,
    string? MetadataJson,
    Guid ClassPerformanceInsightId,
    string? ClassPerformanceInsightCode,
    string? ClassPerformanceInsightName,
    Guid SubjectId,
    string? SubjectCode,
    string? SubjectName);

    public sealed record Query(Guid TenantId, Guid ParentId)
        : IRequest<Result<IReadOnlyCollection<Response>>>;

    public interface IGetTopicPerformanceInsightByClassPerformanceInsightIdQuery
    {
        Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid parentId,
            CancellationToken cancellationToken);
    }

    internal sealed class GetTopicPerformanceInsightByClassPerformanceInsightIdQuery(IDbConnectionFactory connectionFactory)
        : IGetTopicPerformanceInsightByClassPerformanceInsightIdQuery
    {
        public async Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid parentId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                    SELECT
                        tenant_id AS "TenantId",
                        entity.topic_performance_insight_id AS "Id",
                        entity.code AS "Code",
                        entity.name AS "Name",
                        entity.metadata_json AS "MetadataJson",
                        p1.class_performance_insight_id AS "ClassPerformanceInsightId",
                        p1.code AS "ClassPerformanceInsightCode",
                        p1.name AS "ClassPerformanceInsightName",
                        p2.subject_id AS "SubjectId",
                        p2.code AS "SubjectCode",
                        p2.name AS "SubjectName"
                    FROM ai.topic_performance_insight AS entity
                    LEFT JOIN ai.class_performance_insight AS p1
                        ON p1.class_performance_insight_id = entity.class_performance_insight_id
                    LEFT JOIN academic.subject AS p2
                        ON p2.subject_id = entity.subject_id
                    WHERE tenant_id = @TenantId
                      AND entity.class_performance_insight_id = @ParentId
                      AND is_active = TRUE;
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

    public sealed class Handler(IGetTopicPerformanceInsightByClassPerformanceInsightIdQuery query)
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
                "/api/aiprediction/topic-performance-insight/by-class-performance-insight/{parentId:guid}",
                async (Guid parentId, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Query, Result<IReadOnlyCollection<Response>>>(
                        new Query(tenantId, parentId),
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetTopicPerformanceInsightByClassPerformanceInsightId")
            .WithTags("AIPrediction")
            .RequireAuthorization();

        return endpoints;
    }
}
