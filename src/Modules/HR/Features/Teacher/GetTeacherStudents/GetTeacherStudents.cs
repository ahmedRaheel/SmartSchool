using SmartSchool.Application.Messaging;
using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Teacher.GetTeacherStudents;

public static class GetTeacherStudents
{
    public sealed record Request(Guid TenantId, Guid EmployeeId) : IRequest<Response>;
    public sealed record Item(Guid Id, string Name, string Reg, string Section, string Status, Guid ClassSectionId);
    public sealed record Response(IReadOnlyList<Item> Items);

    public interface IGetTeacherStudentsQuery
    {
        Task<Response> ExecuteAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken);
    }

    internal sealed class GetTeacherStudentsQuery(IDbConnectionFactory connectionFactory, ICurrentUser user) : IGetTeacherStudentsQuery
    {
        public async Task<Response> ExecuteAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT DISTINCT s.student_id AS "Id", CONCAT_WS(' ', s.first_name, s.last_name) AS "Name",
                    s.student_number AS "Reg", cs.name AS "Section", s.status AS "Status", cs.class_section_id AS "ClassSectionId"
                FROM academic.teacher_course_assignment a
                JOIN student.student_enrollment se ON se.class_section_id = a.class_section_id AND se.tenant_id = a.tenant_id
                JOIN student.student s ON s.student_id = se.student_id AND s.tenant_id = a.tenant_id
                JOIN academic.class_section cs ON cs.class_section_id = se.class_section_id AND cs.tenant_id = a.tenant_id
                WHERE (@ScopeBranchId IS NULL OR cs.campus_id = @ScopeBranchId) AND a.tenant_id = @TenantId AND a.employee_id = @EmployeeId AND a.is_active = TRUE
                    AND se.is_active = TRUE AND se.status = 'ACTIVE' AND s.is_active = TRUE
                    AND (a.effective_from IS NULL OR a.effective_from <= CURRENT_DATE) AND (a.effective_to IS NULL OR a.effective_to >= CURRENT_DATE)
                ORDER BY "Name";
                """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var rows = await connection.QueryAsync<Item>(new CommandDefinition(sql, new { TenantId = tenantId, EmployeeId = employeeId, ScopeBranchId = user.BranchId }, cancellationToken: cancellationToken));
            return new Response(rows.ToList());
        }
    }

    public sealed class Handler(IGetTeacherStudentsQuery query) : IRequestHandler<Request, Response>
    {
        public Task<Response> HandleAsync(Request request, CancellationToken cancellationToken) => query.ExecuteAsync(request.TenantId, request.EmployeeId, cancellationToken);
    }

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/{employeeId:guid}/students", HandleAsync);
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
