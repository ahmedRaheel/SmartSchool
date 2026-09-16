using Dapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Examinations.Models;
using SmartSchool.Modules.Examinations.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Examinations.Features.StudentExamResult;

public static class GetExamResults
{
    public sealed record Query(Guid TenantId, Guid ExamId) : IRequest<Result<Response>>;
    public sealed record Subject(Guid Id, string Name, decimal TotalMarks, decimal? PassingMarks, DateOnly? ExamDate);
    public sealed record Row(Guid StudentId, string StudentName, string StudentNumber, Guid ExamSubjectId,
        Guid? ResultId, decimal? MarksObtained, decimal? Percentage, string? Grade, bool IsAbsent, string? Remarks);
    public sealed record Response(IReadOnlyList<Subject> Subjects, IReadOnlyList<Row> Rows);
    public interface IGetExamResultsQuery { Task<Response> GetAsync(Query request, CancellationToken cancellationToken); }
    internal sealed class GetExamResultsQuery(IDbConnectionFactory factory, ICurrentUser user) : IGetExamResultsQuery
    {
        public async Task<Response> GetAsync(Query request, CancellationToken cancellationToken)
        {
            const string subjectsSql = """
                SELECT es.exam_subject_id AS "Id", es.name AS "Name", es.total_marks AS "TotalMarks",
                    es.passing_marks AS "PassingMarks", es.exam_date AS "ExamDate"
                FROM exam.exam_subject es JOIN exam.exam e ON e.exam_id = es.exam_id AND e.tenant_id = es.tenant_id
                WHERE e.tenant_id = @TenantId AND e.exam_id = @ExamId AND e.is_active AND es.is_active
                    AND (@CampusId IS NULL OR e.campus_id = @CampusId) AND (@Manage OR e.status = 'PUBLISHED')
                ORDER BY es.exam_date, es.name;
                """;
            const string rowsSql = """
                SELECT s.student_id AS "StudentId", trim(s.first_name || ' ' || coalesce(s.last_name, '')) AS "StudentName",
                    coalesce(s.student_number, '') AS "StudentNumber", es.exam_subject_id AS "ExamSubjectId",
                    r.student_exam_result_id AS "ResultId", r.marks_obtained AS "MarksObtained", r.percentage AS "Percentage",
                    r.grade AS "Grade", coalesce(r.is_absent, false) AS "IsAbsent", r.remarks AS "Remarks"
                FROM exam.exam e JOIN exam.exam_subject es ON es.exam_id = e.exam_id AND es.tenant_id = e.tenant_id AND es.is_active
                JOIN student.student_enrollment en ON en.tenant_id = e.tenant_id AND en.class_section_id = e.class_section_id AND en.is_active AND en.status = 'ACTIVE'
                JOIN student.student s ON s.student_id = en.student_id AND s.tenant_id = en.tenant_id AND s.is_active
                LEFT JOIN exam.student_exam_result r ON r.tenant_id = e.tenant_id AND r.exam_subject_id = es.exam_subject_id AND r.student_id = s.student_id AND r.is_active
                WHERE e.tenant_id = @TenantId AND e.exam_id = @ExamId AND e.is_active
                    AND (@CampusId IS NULL OR e.campus_id = @CampusId)
                    AND (@Manage OR (e.status = 'PUBLISHED' AND (s.student_id = @StudentId OR EXISTS (
                        SELECT 1 FROM student.student_guardian sg JOIN student.guardian g ON g.guardian_id = sg.guardian_id AND g.tenant_id = sg.tenant_id
                        WHERE sg.tenant_id = s.tenant_id AND sg.student_id = s.student_id AND sg.is_active AND sg.can_view_academics
                            AND g.is_active AND g.user_id = @UserId))))
                ORDER BY s.first_name, s.last_name, s.student_id, es.name;
                """;
            var parameters = new { request.TenantId, request.ExamId, CampusId = user.BranchId,
                Manage = Authorization.ExamPermissions.CanManage(user), user.StudentId, user.UserId };
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            var rows = (await connection.QueryAsync<Row>(new CommandDefinition(rowsSql, parameters, cancellationToken: cancellationToken))).AsList();
            var subjects = (await connection.QueryAsync<Subject>(new CommandDefinition(subjectsSql, parameters, cancellationToken: cancellationToken))).AsList();
            // A student's subjects are only exposed for exams in that student's visible roster.
            if (!Authorization.ExamPermissions.CanManage(user) && rows.Count == 0) subjects.Clear();
            return new(subjects, rows);
        }
    }
    public sealed class Handler(IGetExamResultsQuery query) : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Query request, CancellationToken cancellationToken) =>
            Result<Response>.Success(await query.GetAsync(request, cancellationToken));
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/examinations/exam/{examId:guid}/results", async (Guid examId, Guid? tenantId,
            ITenantScope scope, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var tenant = scope.Resolve(tenantId);
            if (!tenant.HasValue) return Results.BadRequest(new { message = "Select a tenant." });
            return (await mediator.SendAsync<Query, Result<Response>>(new(tenant.Value, examId), cancellationToken)).ToHttpResult();
        }).WithName("GetExamResults").WithTags("Examinations").RequireAuthorization();
        return endpoints;
    }
}
