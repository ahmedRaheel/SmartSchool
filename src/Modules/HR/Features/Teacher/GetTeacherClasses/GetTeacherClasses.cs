using SmartSchool.Application.Messaging;
using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Teacher.GetTeacherClasses;

public static class GetTeacherClasses
{
    public sealed record Request(Guid TenantId, Guid EmployeeId) : IRequest<Response>;
    public sealed record Item(Guid Id, Guid ClassSectionId, string Subject, string SubjectCode, string ClassSection, string Campus, string Role, int PeriodsPerWeek, long TotalStudents, long PendingAssignments);
    public sealed record Response(IReadOnlyList<Item> Items);

    public interface IGetTeacherClassesQuery
    {
        Task<Response> ExecuteAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken);
    }

    internal sealed class GetTeacherClassesQuery(IDbConnectionFactory connectionFactory, ICurrentUser user) : IGetTeacherClassesQuery
    {
        public async Task<Response> ExecuteAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT a.teacher_course_assignment_id AS "Id", a.class_section_id AS "ClassSectionId",
                    s.name AS "Subject", s.code AS "SubjectCode", cs.name AS "ClassSection", c.name AS "Campus",
                    a.assignment_role AS "Role", a.periods_per_week AS "PeriodsPerWeek",
                    (SELECT COUNT(DISTINCT se.student_id) FROM student.student_enrollment se
                        WHERE se.tenant_id = a.tenant_id AND se.class_section_id = a.class_section_id AND se.is_active = TRUE AND se.status = 'ACTIVE') AS "TotalStudents",
                    (SELECT COUNT(*) FROM lms.student_assignment_submission sub
                        JOIN lms.academic_assignment ass ON ass.academic_assignment_id = sub.academic_assignment_id AND ass.tenant_id = sub.tenant_id
                        WHERE ass.tenant_id = a.tenant_id AND ass.teacher_employee_id = a.employee_id
                            AND ass.class_section_id = a.class_section_id AND sub.status IN ('SUBMITTED', 'LATE')) AS "PendingAssignments"
                FROM academic.teacher_course_assignment a
                JOIN academic.class_section cs ON cs.class_section_id = a.class_section_id AND cs.tenant_id = a.tenant_id
                JOIN academic.course_offering co ON co.course_offering_id = a.course_offering_id AND co.tenant_id = a.tenant_id
                JOIN academic.subject s ON s.subject_id = co.subject_id AND s.tenant_id = a.tenant_id
                JOIN org.campus c ON c.campus_id = cs.campus_id AND c.tenant_id = a.tenant_id
                WHERE (@ScopeBranchId IS NULL OR cs.campus_id = @ScopeBranchId) AND a.tenant_id = @TenantId AND a.employee_id = @EmployeeId AND a.is_active = TRUE
                    AND (a.effective_from IS NULL OR a.effective_from <= CURRENT_DATE) AND (a.effective_to IS NULL OR a.effective_to >= CURRENT_DATE)
                ORDER BY cs.name, s.name;
                """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var rows = await connection.QueryAsync<Item>(new CommandDefinition(sql, new { TenantId = tenantId, EmployeeId = employeeId, ScopeBranchId = user.BranchId }, cancellationToken: cancellationToken));
            return new Response(rows.ToList());
        }
    }

    public sealed class Handler(IGetTeacherClassesQuery query) : IRequestHandler<Request, Response>
    {
        public Task<Response> HandleAsync(Request request, CancellationToken cancellationToken) => query.ExecuteAsync(request.TenantId, request.EmployeeId, cancellationToken);
    }

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/{employeeId:guid}/classes", HandleAsync);
    }

    private static async Task<IResult> HandleAsync(Guid employeeId, Guid? tenantId, ITenantScope tenantScope, ICurrentUser currentUser, IMediator mediator, CancellationToken cancellationToken)
    {
            if (currentUser.IsInRole(SmartSchoolRoles.Teacher) &&
                employeeId != (currentUser.EmployeeId ?? currentUser.TeacherId))
            {
                return Results.Forbid();
            }
            var resolvedTenantId = tenantScope.Resolve(tenantId);
            if (!resolvedTenantId.HasValue)
            {
                return Results.BadRequest(new { message = "Tenant is required." });
            }

            var response = await mediator.SendAsync<Request, Response>(new Request(resolvedTenantId.Value, employeeId), cancellationToken);
            return Results.Ok(response.Items);
    }
}
