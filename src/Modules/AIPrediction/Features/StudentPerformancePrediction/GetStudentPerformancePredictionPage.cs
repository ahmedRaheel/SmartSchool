using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Features.StudentPerformancePrediction;

public static class GetStudentPerformancePredictionPage
{
    /// <summary>
    /// Represents the response returned by this StudentPerformancePredictionEntity feature.
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
    Guid AcademicYearId,
    string? AcademicYearCode,
    string? AcademicYearName,
    Guid CourseOfferingId,
    string? CourseOfferingCode,
    string? CourseOfferingName,
    Guid? PredictionModelId,
    string? PredictionModelCode,
    string? PredictionModelName,
    Guid SubjectId,
    string? SubjectCode,
    string? SubjectName,
    Guid? TargetExamId,
    string? TargetExamCode,
    string? TargetExamName,
    Guid? TargetExamSubjectId,
    string? TargetExamSubjectCode,
    string? TargetExamSubjectName,
    Guid? TermId,
    string? TermCode,
    string? TermName);

    public sealed record Query(
        Guid TenantId,
        int Page = 1,
        int PageSize = 25) : IRequest<Result<PagedResult<Response>>>;

    public interface IGetStudentPerformancePredictionPageQuery
    {
        Task<PagedResult<Response>> GetPageAsync(
                Guid tenantId,
                int page,
                int pageSize,
                CancellationToken cancellationToken);

    }

    internal sealed class GetStudentPerformancePredictionPageQuery(

        IDbConnectionFactory connectionFactory) : IGetStudentPerformancePredictionPageQuery
    {
        public async Task<PagedResult<Response>> GetPageAsync(
                Guid tenantId,
                int page,
                int pageSize,
                CancellationToken cancellationToken)
            {
                const string countSql = """
                    SELECT COUNT(*)
                    FROM ai.student_performance_prediction AS entity
                    WHERE entity.tenant_id = @TenantId
                      AND entity.is_active = TRUE;
                    """;

                const string pageSql = """
                    SELECT
                    entity.tenant_id AS "TenantId",
                    entity.student_performance_prediction_id AS "Id",
                    entity.code AS "Code",
                    entity.name AS "Name",
                    entity.metadata_json AS "MetadataJson",
                        p1.academic_year_id AS "AcademicYearId",
                        p1.code AS "AcademicYearCode",
                        p1.name AS "AcademicYearName",
                        p2.course_offering_id AS "CourseOfferingId",
                        p2.code AS "CourseOfferingCode",
                        p2.name AS "CourseOfferingName",
                        p3.prediction_model_id AS "PredictionModelId",
                        p3.code AS "PredictionModelCode",
                        p3.name AS "PredictionModelName",
                        p4.subject_id AS "SubjectId",
                        p4.code AS "SubjectCode",
                        p4.name AS "SubjectName",
                        p5.exam_id AS "TargetExamId",
                        p5.code AS "TargetExamCode",
                        p5.name AS "TargetExamName",
                        p6.exam_subject_id AS "TargetExamSubjectId",
                        p6.code AS "TargetExamSubjectCode",
                        p6.name AS "TargetExamSubjectName",
                        p7.term_id AS "TermId",
                        p7.code AS "TermCode",
                        p7.name AS "TermName"
                    FROM ai.student_performance_prediction AS entity
                    LEFT JOIN academic.academic_year AS p1
                        ON p1.academic_year_id = entity.academic_year_id
                    LEFT JOIN academic.course_offering AS p2
                        ON p2.course_offering_id = entity.course_offering_id
                    LEFT JOIN ai.prediction_model AS p3
                        ON p3.prediction_model_id = entity.prediction_model_id
                    LEFT JOIN academic.subject AS p4
                        ON p4.subject_id = entity.subject_id
                    LEFT JOIN exam.exam AS p5
                        ON p5.exam_id = entity.target_exam_id
                    LEFT JOIN exam.exam_subject AS p6
                        ON p6.exam_subject_id = entity.target_exam_subject_id
                    LEFT JOIN academic.term AS p7
                        ON p7.term_id = entity.term_id
                    WHERE entity.tenant_id = @TenantId
                      AND entity.is_active = TRUE
                    ORDER BY entity.student_performance_prediction_id
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
                        cancellationToken: cancellationToken));

                var items = (await connection.QueryAsync<Response>(
                    new CommandDefinition(
                        pageSql,
                        parameters,
                        cancellationToken: cancellationToken)))
                    .AsList();

                return new PagedResult<Response>(
                    items,
                    page,
                    pageSize,
                    totalCount);
            }
    }

    public sealed class Handler(IGetStudentPerformancePredictionPageQuery query)
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
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "student-performance-prediction"),
                async (Guid tenantId, int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, page, pageSize);
                    var result = await mediator.SendAsync<Query, Result<PagedResult<Response>>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetStudentPerformancePredictionPage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
