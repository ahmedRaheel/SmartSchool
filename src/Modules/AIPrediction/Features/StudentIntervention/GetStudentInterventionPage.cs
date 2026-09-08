using SmartSchool.Application.Persistence;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Features.StudentIntervention;

public static class GetStudentInterventionPage
{
    /// <summary>
    /// Represents the response returned by this StudentInterventionEntity feature.
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
    Guid? CourseOfferingId,
    string? CourseOfferingCode,
    string? CourseOfferingName,
    Guid? SourcePredictionId,
    string? SourcePredictionCode,
    string? SourcePredictionName,
    Guid? SourceRecommendationId,
    string? SourceRecommendationCode,
    string? SourceRecommendationName,
    Guid? SubjectId,
    string? SubjectCode,
    string? SubjectName);

    public sealed record Query(
        Guid TenantId,
        int Page = 1,
        int PageSize = 25) : IRequest<Result<PagedResult<Response>>>;

    public interface IGetStudentInterventionPageQuery
    {
        Task<PagedResult<Response>> GetPageAsync(
                Guid tenantId,
                int page,
                int pageSize,
                CancellationToken cancellationToken);

    }

    internal sealed class GetStudentInterventionPageQuery(
        IDbConnectionFactory connectionFactory) : IGetStudentInterventionPageQuery
    {
        public async Task<PagedResult<Response>> GetPageAsync(
                Guid tenantId,
                int page,
                int pageSize,
                CancellationToken cancellationToken)
            {
                const string countSql = """
                    SELECT COUNT(*)
                    FROM ai.student_intervention AS entity
                    WHERE entity.tenant_id = @TenantId
                      AND entity.is_active = TRUE;
                    """;

                const string pageSql = """
                    SELECT
                    entity.tenant_id AS "TenantId",
                    entity.student_intervention_id AS "Id",
                    entity.code AS "Code",
                    entity.name AS "Name",
                    entity.metadata_json AS "MetadataJson",
                        p1.course_offering_id AS "CourseOfferingId",
                        p1.code AS "CourseOfferingCode",
                        p1.name AS "CourseOfferingName",
                        p2.student_performance_prediction_id AS "SourcePredictionId",
                        p2.code AS "SourcePredictionCode",
                        p2.name AS "SourcePredictionName",
                        p3.teaching_recommendation_id AS "SourceRecommendationId",
                        p3.code AS "SourceRecommendationCode",
                        p3.name AS "SourceRecommendationName",
                        p4.subject_id AS "SubjectId",
                        p4.code AS "SubjectCode",
                        p4.name AS "SubjectName"
                    FROM ai.student_intervention AS entity
                    LEFT JOIN academic.course_offering AS p1
                        ON p1.course_offering_id = entity.course_offering_id
                    LEFT JOIN ai.student_performance_prediction AS p2
                        ON p2.student_performance_prediction_id = entity.source_prediction_id
                    LEFT JOIN ai.teaching_recommendation AS p3
                        ON p3.teaching_recommendation_id = entity.source_recommendation_id
                    LEFT JOIN academic.subject AS p4
                        ON p4.subject_id = entity.subject_id
                    WHERE entity.tenant_id = @TenantId
                      AND entity.is_active = TRUE
                    ORDER BY entity.student_intervention_id
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

    public sealed class Handler(IGetStudentInterventionPageQuery query)
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
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "student-intervention"),
                async (Guid tenantId, int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, page, pageSize);
                    var result = await mediator.SendAsync<Query, Result<PagedResult<Response>>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetStudentInterventionPage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
