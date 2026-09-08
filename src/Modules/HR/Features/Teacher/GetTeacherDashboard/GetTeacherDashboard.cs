using SmartSchool.Application.Messaging;
using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Teacher.GetTeacherDashboard;

public static class GetTeacherDashboard
{
    public sealed record Request(Guid TenantId, Guid EmployeeId) : IRequest<Response>;
    public sealed record Response(IReadOnlyList<dynamic> Items);

    public interface IGetTeacherDashboardQuery
    {
        Task<Response> ExecuteAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken);
    }

    internal sealed class GetTeacherDashboardQuery(IDbConnectionFactory connectionFactory) : IGetTeacherDashboardQuery
    {
        public async Task<Response> ExecuteAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken)
        {
            const string sql = """
            SELECT @EmployeeId AS "EmployeeId", (SELECT COUNT(*) FROM academic.teacher_course_assignment WHERE tenant_id = @TenantId AND employee_id = @EmployeeId AND (effective_to IS NULL OR effective_to >= CURRENT_DATE)) AS "ActiveCourseAssignments", (SELECT COUNT(*) FROM lms.academic_assignment WHERE tenant_id = @TenantId AND teacher_employee_id = @EmployeeId AND status IN ('DRAFT','PUBLISHED','ACTIVE')) AS "Assignments", (SELECT COUNT(*) FROM lms.student_assignment_submission s JOIN lms.academic_assignment a ON a.academic_assignment_id = s.academic_assignment_id WHERE a.tenant_id = @TenantId AND a.teacher_employee_id = @EmployeeId AND s.status IN ('SUBMITTED','PENDING_REVIEW')) AS "SubmissionsToGrade", (SELECT COUNT(DISTINCT se.student_id) FROM academic.teacher_course_assignment a JOIN student.student_enrollment se ON se.class_section_id = a.class_section_id AND se.tenant_id = a.tenant_id WHERE a.tenant_id = @TenantId AND a.employee_id = @EmployeeId) AS "Students";
            """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var rows = await connection.QueryAsync(new CommandDefinition(sql, new { TenantId = tenantId, EmployeeId = employeeId }, cancellationToken: cancellationToken));
            return new Response(rows.ToList());
        }
    }

    public sealed class Handler(IGetTeacherDashboardQuery query) : IRequestHandler<Request, Response>
    {
        public Task<Response> HandleAsync(Request request, CancellationToken cancellationToken) => query.ExecuteAsync(request.TenantId, request.EmployeeId, cancellationToken);
    }

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/{employeeId:guid}/dashboard", HandleAsync);
    }

    private static async Task<IResult> HandleAsync(Guid employeeId, Guid? tenantId, ITenantScope tenantScope, IMediator mediator, CancellationToken cancellationToken)
    {
            var resolvedTenantId = tenantScope.IsSuperAdmin ? tenantId : tenantScope.Resolve(tenantId);
            if (!resolvedTenantId.HasValue)
            {
                return Results.BadRequest(new { message = "Tenant is required." });
            }

            var response = await mediator.SendAsync<Request, Response>(new Request(resolvedTenantId.Value, employeeId), cancellationToken);
            return Results.Ok(response.Items);
    }
}
