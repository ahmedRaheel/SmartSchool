using Dapper;
using SmartSchool.Application.Persistence;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIPrediction.Models;

using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIPrediction.Features.StudentPerformancePrediction;

public static class GetStudentPerformancePredictionById
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
        Guid Id) : IRequest<Result<Response>>;    public interface IGetStudentPerformancePredictionByIdQuery
    {
        Task<Result<Response>> ExecuteAsync(
            Query request,
            CancellationToken cancellationToken);
    }



    internal sealed class GetStudentPerformancePredictionByIdQuery(IDbConnectionFactory connectionFactory) : IGetStudentPerformancePredictionByIdQuery
    {
        public async Task<Result<Response>> ExecuteAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT entity.tenant_id AS "TenantId", entity.student_performance_prediction_id AS "Id", entity.code AS "Code", entity.name AS "Name", entity.metadata_json::text AS "MetadataJson",
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
                  AND entity.student_performance_prediction_id = @Id
                  AND entity.is_active = TRUE;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var response = await connection.QuerySingleOrDefaultAsync<Response>(
                new CommandDefinition(sql, new { request.TenantId, request.Id }, cancellationToken: cancellationToken));

            if (response is null)
            {
                return Result<Response>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Response))));
            }

            return Result<Response>.Success(response);
        }
    }

    public sealed class Handler(IGetStudentPerformancePredictionByIdQuery query)
        : IRequestHandler<Query, Result<Response>>
    {
        public Task<Result<Response>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            return query.ExecuteAsync(request, cancellationToken);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "student-performance-prediction"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetStudentPerformancePredictionById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
