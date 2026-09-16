using Dapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Learning.Authorization;
using SmartSchool.Modules.Learning.Models;
using SmartSchool.Modules.Learning.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Learning.Features.AssignmentSubmission;

public static class GetAssignmentSubmissionPage
{
    public sealed record Query(Guid TenantId, Guid AssignmentId) : IRequest<Result<Response>>;
    public sealed record Item(Guid? Id, Guid StudentId, string StudentName, int? AttemptNo,
        DateTimeOffset? SubmittedAt, string? FileName, string? Comment, decimal? Marks,
        string? Feedback, string Status);
    public sealed record Response(IReadOnlyList<Item> Items);
    public interface IGetAssignmentSubmissionPageQuery { Task<Response> GetAsync(Query request, CancellationToken cancellationToken); }
    internal sealed class GetAssignmentSubmissionPageQuery(IDbConnectionFactory factory, ICurrentUser user) : IGetAssignmentSubmissionPageQuery
    {
        public async Task<Response> GetAsync(Query request, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT sub.submission_id AS "Id", s.student_id AS "StudentId",
                    trim(s.first_name || ' ' || coalesce(s.last_name, '')) AS "StudentName",
                    sub.attempt_no AS "AttemptNo", sub.submitted_at AS "SubmittedAt", sub.file_name AS "FileName",
                    sub.submission_text AS "Comment", sub.marks_obtained AS "Marks", sub.teacher_feedback AS "Feedback",
                    coalesce(sub.status, 'MISSING') AS "Status"
                FROM lms.academic_assignment a
                JOIN student.student_enrollment en ON en.tenant_id = a.tenant_id
                    AND en.class_section_id = a.class_section_id AND en.is_active AND en.status = 'ACTIVE'
                JOIN student.student s ON s.student_id = en.student_id AND s.tenant_id = en.tenant_id AND s.is_active
                LEFT JOIN LATERAL (SELECT x.* FROM lms.student_assignment_submission x
                    WHERE x.tenant_id = a.tenant_id AND x.academic_assignment_id = a.academic_assignment_id
                        AND x.student_id = s.student_id AND x.is_active ORDER BY x.attempt_no DESC LIMIT 1) sub ON TRUE
                WHERE a.tenant_id = @TenantId AND a.academic_assignment_id = @AssignmentId AND a.is_active
                    AND (@CampusId IS NULL OR a.branch_id = @CampusId)
                    AND (@ManageAll OR a.teacher_employee_id = @EmployeeId OR s.student_id = @StudentId
                        OR EXISTS (SELECT 1 FROM student.student_guardian sg
                            JOIN student.guardian g ON g.guardian_id = sg.guardian_id AND g.tenant_id = sg.tenant_id
                            WHERE sg.tenant_id = a.tenant_id AND sg.student_id = s.student_id
                                AND sg.is_active AND sg.can_view_academics AND g.is_active AND g.user_id = @UserId))
                ORDER BY s.first_name, s.last_name, s.student_id;
                """;
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            var rows = await connection.QueryAsync<Item>(new CommandDefinition(sql, new { request.TenantId, request.AssignmentId,
                CampusId = user.BranchId, ManageAll = LearningPermissions.CanManageAll(user),
                EmployeeId = user.IsInRole(SmartSchoolRoles.Teacher) ? user.EmployeeId ?? user.TeacherId : null,
                user.StudentId, user.UserId }, cancellationToken: cancellationToken));
            return new(rows.AsList());
        }
    }
    public sealed class Handler(IGetAssignmentSubmissionPageQuery query) : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Query request, CancellationToken cancellationToken) => Result<Response>.Success(await query.GetAsync(request, cancellationToken));
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/learning/assignment-submission", async (Guid assignmentId, Guid? tenantId,
            ITenantScope scope, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var tenant = scope.Resolve(tenantId);
            if (!tenant.HasValue) return Results.BadRequest(new { message = "Select a tenant." });
            return (await mediator.SendAsync<Query, Result<Response>>(new(tenant.Value, assignmentId), cancellationToken)).ToHttpResult();
        }).WithName("GetAssignmentSubmissionPage").WithTags("Learning").RequireAuthorization();
        return endpoints;
    }
}
