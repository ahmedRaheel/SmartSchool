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

namespace SmartSchool.Modules.Learning.Features.Assignment;

public static class GetAssignmentPage
{
    public sealed record Query(Guid TenantId, int Page = 1, int PageSize = 25) : IRequest<Result<PagedResult<Item>>>;
    public sealed record Item(Guid Id, string Code, string Name, string AssignmentTypeCode,
        string? Description, string ClassSection, string Course, Guid TeacherEmployeeId,
        DateTimeOffset? DueAt, decimal? TotalMarks, bool AllowLateSubmission, int MaxAttempts,
        string Status, long SubmissionCount, Guid? MySubmissionId, string? MySubmissionStatus,
        decimal? MyMarks, string? MyFeedback);
    public interface IGetAssignmentPageQuery { Task<PagedResult<Item>> GetAsync(Query request, CancellationToken cancellationToken); }
    internal sealed class GetAssignmentPageQuery(IDbConnectionFactory factory, ICurrentUser user) : IGetAssignmentPageQuery
    {
        public async Task<PagedResult<Item>> GetAsync(Query request, CancellationToken cancellationToken)
        {
            const string filter = """
                FROM lms.academic_assignment a
                JOIN academic.class_section cs ON cs.class_section_id = a.class_section_id AND cs.tenant_id = a.tenant_id
                JOIN academic.course_offering co ON co.course_offering_id = a.course_offering_id AND co.tenant_id = a.tenant_id
                WHERE a.tenant_id = @TenantId AND a.is_active
                    AND (@CampusId IS NULL OR a.branch_id = @CampusId)
                    AND (@ManageAll OR a.teacher_employee_id = @EmployeeId
                        OR EXISTS (SELECT 1 FROM student.student_enrollment en
                            WHERE en.tenant_id = a.tenant_id AND en.class_section_id = a.class_section_id
                                AND en.is_active AND en.status = 'ACTIVE'
                                AND (en.student_id = @StudentId OR EXISTS (
                                    SELECT 1 FROM student.student_guardian sg
                                    JOIN student.guardian g ON g.guardian_id = sg.guardian_id AND g.tenant_id = sg.tenant_id
                                    WHERE sg.student_id = en.student_id AND sg.tenant_id = a.tenant_id
                                        AND sg.is_active AND sg.can_view_academics AND g.is_active AND g.user_id = @UserId))))
                """;
            const string projection = """
                SELECT a.academic_assignment_id AS "Id", a.code AS "Code", a.name AS "Name",
                    a.assignment_type_code AS "AssignmentTypeCode", a.description AS "Description",
                    cs.name AS "ClassSection", coalesce(co.display_name, co.name) AS "Course",
                    a.teacher_employee_id AS "TeacherEmployeeId", a.due_at AS "DueAt",
                    a.total_marks AS "TotalMarks", a.allow_late_submission AS "AllowLateSubmission",
                    a.max_attempts AS "MaxAttempts", a.status AS "Status",
                    (SELECT count(DISTINCT s.student_id) FROM lms.student_assignment_submission s
                        WHERE s.tenant_id = a.tenant_id AND s.academic_assignment_id = a.academic_assignment_id AND s.is_active) AS "SubmissionCount",
                    (SELECT s.submission_id FROM lms.student_assignment_submission s WHERE s.tenant_id = a.tenant_id
                        AND s.academic_assignment_id = a.academic_assignment_id AND s.student_id = @StudentId AND s.is_active
                        ORDER BY s.attempt_no DESC LIMIT 1) AS "MySubmissionId",
                    (SELECT s.status FROM lms.student_assignment_submission s WHERE s.tenant_id = a.tenant_id
                        AND s.academic_assignment_id = a.academic_assignment_id AND s.student_id = @StudentId AND s.is_active
                        ORDER BY s.attempt_no DESC LIMIT 1) AS "MySubmissionStatus",
                    (SELECT s.marks_obtained FROM lms.student_assignment_submission s WHERE s.tenant_id = a.tenant_id
                        AND s.academic_assignment_id = a.academic_assignment_id AND s.student_id = @StudentId AND s.is_active
                        ORDER BY s.attempt_no DESC LIMIT 1) AS "MyMarks",
                    (SELECT s.teacher_feedback FROM lms.student_assignment_submission s WHERE s.tenant_id = a.tenant_id
                        AND s.academic_assignment_id = a.academic_assignment_id AND s.student_id = @StudentId AND s.is_active
                        ORDER BY s.attempt_no DESC LIMIT 1) AS "MyFeedback"
                """;
            var page = Math.Max(1, request.Page);
            var size = Math.Clamp(request.PageSize, 1, 100);
            var parameters = new { request.TenantId, CampusId = user.BranchId,
                ManageAll = LearningPermissions.CanManageAll(user),
                EmployeeId = user.IsInRole(SmartSchoolRoles.Teacher) ? user.EmployeeId ?? user.TeacherId : null,
                user.StudentId, user.UserId, Size = size, Offset = (page - 1) * size };
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            var count = await connection.ExecuteScalarAsync<long>(new CommandDefinition("SELECT COUNT(*) " + filter, parameters, cancellationToken: cancellationToken));
            var rows = await connection.QueryAsync<Item>(new CommandDefinition(projection + filter +
                " ORDER BY a.assigned_at DESC, a.academic_assignment_id LIMIT @Size OFFSET @Offset", parameters, cancellationToken: cancellationToken));
            return new(rows.AsList(), page, size, count);
        }
    }
    public sealed class Handler(IGetAssignmentPageQuery query) : IRequestHandler<Query, Result<PagedResult<Item>>>
    {
        public async Task<Result<PagedResult<Item>>> HandleAsync(Query request, CancellationToken cancellationToken) =>
            Result<PagedResult<Item>>.Success(await query.GetAsync(request, cancellationToken));
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/learning/assignment", async (Guid? tenantId, int? page, int? pageSize,
            ITenantScope scope, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var tenant = scope.Resolve(tenantId);
            if (!tenant.HasValue) return Results.BadRequest(new { message = "Select a tenant." });
            return (await mediator.SendAsync<Query, Result<PagedResult<Item>>>(new(tenant.Value, page ?? 1, pageSize ?? 25), cancellationToken)).ToHttpResult();
        }).WithName("GetAssignmentPage").WithTags("Learning").RequireAuthorization();
        return endpoints;
    }
}
