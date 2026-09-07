using SmartSchool.Application.Persistence;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Features.TopicPerformanceInsight;

public static class GetTopicPerformanceInsightPage
{
    /// <summary>
    /// Represents the response returned by this TopicPerformanceInsightEntity feature.
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
    Guid ClassPerformanceInsightId,
    string? ClassPerformanceInsightCode,
    string? ClassPerformanceInsightName,
    Guid SubjectId,
    string? SubjectCode,
    string? SubjectName);

    public sealed record Query(
        Guid TenantId,
        int Page = 1,
        int PageSize = 25) : IRequest<Result<PagedResult<Response>>>;

    public interface IGetTopicPerformanceInsightPageQuery
    {
        Task<PagedResult<Response>> GetPageAsync(
                Guid tenantId,
                int page,
                int pageSize,
                CancellationToken cancellationToken);

    }

    internal sealed class GetTopicPerformanceInsightPageQuery(
        IDbConnectionFactory connectionFactory) : IGetTopicPerformanceInsightPageQuery
    {
        public async Task<PagedResult<Response>> GetPageAsync(
                Guid tenantId,
                int page,
                int pageSize,
                CancellationToken cancellationToken)
            {
                const string countSql = """
                    SELECT COUNT(*)
                    FROM ai.topic_performance_insight AS entity
                    WHERE tenant_id = @TenantId
                      AND is_active = TRUE;
                    """;

                const string pageSql = """
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
                      AND is_active = TRUE
                    ORDER BY entity.topic_performance_insight_id
                    LIMIT @PageSize OFFSET @Offset;
                    """;

                await using var connection =
                    await connectionFactory.OpenConnectionAsync(cancellationToken);

                var parameters = new
                {
                    TenantId = tenantId,
                    PageSize = pageSize,
                    Offset = (page - 1) * pageSize
                };

                var totalCount = await connection.ExecuteScalarAsync<long>(
                    new CommandDefinition(
                        countSql,
                        parameters,
                        cancellationToken: cancellationToken)).ConfigureAwait(false);

                var items = (await connection.QueryAsync<Response>(
                    new CommandDefinition(
                        pageSql,
                        parameters,
                        cancellationToken: cancellationToken)).ConfigureAwait(false))
                    .AsList();

                return new PagedResult<Response>(
                    items,
                    page,
                    pageSize,
                    totalCount);
            }
    }

    public sealed class Handler(IGetTopicPerformanceInsightPageQuery query)
        : IRequestHandler<Query, Result<PagedResult<Response>>>
    {
        public async Task<Result<PagedResult<Response>>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var pageRequest = new PageRequest(request.Page, request.PageSize);
            var page = await query.GetPageAsync(
                request.TenantId,
                pageRequest.NormalizedPage,
                pageRequest.NormalizedPageSize,
                cancellationToken);
            var response = new PagedResult<Response>(
                page.Items,
                page.Page,
                page.PageSize,
                page.TotalCount);
            return Result<PagedResult<Response>>.Success(response);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "topic-performance-insight"),
                async (Guid tenantId, int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, page, pageSize);
                    var result = await mediator.SendAsync<Query, Result<PagedResult<Response>>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetTopicPerformanceInsightPage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
