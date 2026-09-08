using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Features.StudentPerformancePrediction;

public static class GetStudentPerformancePredictionByCourseOfferingId
{
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

    public sealed record Query(Guid TenantId, Guid ParentId)
        : IRequest<Result<IReadOnlyCollection<Response>>>;

    public interface IGetStudentPerformancePredictionByCourseOfferingIdQuery
    {
        Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid parentId,
            CancellationToken cancellationToken);
    }

    internal sealed class GetStudentPerformancePredictionByCourseOfferingIdQuery(IDbConnectionFactory connectionFactory)
        : IGetStudentPerformancePredictionByCourseOfferingIdQuery
    {
        public async Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid parentId,
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
                  AND entity.course_offering_id = @ParentId
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

    public sealed class Handler(IGetStudentPerformancePredictionByCourseOfferingIdQuery query)
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
                "/api/aiprediction/student-performance-prediction/by-course-offering/{parentId:guid}",
                async (Guid parentId, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Query, Result<IReadOnlyCollection<Response>>>(
                        new Query(tenantId, parentId),
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetStudentPerformancePredictionByCourseOfferingId")
            .WithTags("AIPrediction")
            .RequireAuthorization();

        return endpoints;
    }
}
