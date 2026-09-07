using SmartSchool.Application.Persistence;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Features.TeachingRecommendation;

public static class GetTeachingRecommendationPage
{
    /// <summary>
    /// Represents the response returned by this TeachingRecommendationEntity feature.
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
    Guid? ClassPerformanceInsightId,
    string? ClassPerformanceInsightCode,
    string? ClassPerformanceInsightName,
    Guid ClassSectionId,
    string? ClassSectionCode,
    string? ClassSectionName,
    Guid CourseOfferingId,
    string? CourseOfferingCode,
    string? CourseOfferingName,
    Guid? SubjectId,
    string? SubjectCode,
    string? SubjectName);

    public sealed record Query(
        Guid TenantId,
        int Page = 1,
        int PageSize = 25) : IRequest<Result<PagedResult<Response>>>;

    public interface IGetTeachingRecommendationPageQuery
    {
        Task<PagedResult<Response>> GetPageAsync(
                Guid tenantId,
                int page,
                int pageSize,
                CancellationToken cancellationToken);

    }

    internal sealed class GetTeachingRecommendationPageQuery(
        IDbConnectionFactory connectionFactory) : IGetTeachingRecommendationPageQuery
    {
        public async Task<PagedResult<Response>> GetPageAsync(
                Guid tenantId,
                int page,
                int pageSize,
                CancellationToken cancellationToken)
            {
                const string countSql = """
                    SELECT COUNT(*)
                    FROM ai.teaching_recommendation AS entity
                    WHERE entity.tenant_id = @TenantId
                      AND entity.is_active = TRUE;
                    """;

                const string pageSql = """
                    SELECT
                    entity.tenant_id AS "TenantId",
                    entity.teaching_recommendation_id AS "Id",
                    entity.code AS "Code",
                    entity.name AS "Name",
                    entity.metadata_json AS "MetadataJson",
                        p1.class_performance_insight_id AS "ClassPerformanceInsightId",
                        p1.code AS "ClassPerformanceInsightCode",
                        p1.name AS "ClassPerformanceInsightName",
                        p2.class_section_id AS "ClassSectionId",
                        p2.code AS "ClassSectionCode",
                        p2.name AS "ClassSectionName",
                        p3.course_offering_id AS "CourseOfferingId",
                        p3.code AS "CourseOfferingCode",
                        p3.name AS "CourseOfferingName",
                        p4.subject_id AS "SubjectId",
                        p4.code AS "SubjectCode",
                        p4.name AS "SubjectName"
                    FROM ai.teaching_recommendation AS entity
                    LEFT JOIN ai.class_performance_insight AS p1
                        ON p1.class_performance_insight_id = entity.class_performance_insight_id
                    LEFT JOIN academic.class_section AS p2
                        ON p2.class_section_id = entity.class_section_id
                    LEFT JOIN academic.course_offering AS p3
                        ON p3.course_offering_id = entity.course_offering_id
                    LEFT JOIN academic.subject AS p4
                        ON p4.subject_id = entity.subject_id
                    WHERE entity.tenant_id = @TenantId
                      AND entity.is_active = TRUE
                    ORDER BY entity.teaching_recommendation_id
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

    public sealed class Handler(IGetTeachingRecommendationPageQuery query)
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
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "teaching-recommendation"),
                async (Guid tenantId, int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, page, pageSize);
                    var result = await mediator.SendAsync<Query, Result<PagedResult<Response>>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetTeachingRecommendationPage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
