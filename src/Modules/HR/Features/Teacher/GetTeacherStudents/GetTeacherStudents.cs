using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Teacher.GetTeacherStudents;

public static class GetTeacherStudents
{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/{employeeId:guid}/students", HandleAsync);
    }

    private static async Task<IResult> HandleAsync(Guid employeeId, Guid? tenantId, ITenantScope tenantScope, IDbConnectionFactory connectionFactory, CancellationToken cancellationToken)
    {
        var resolvedTenantId = (tenantScope.IsSuperAdmin ? tenantId : tenantScope.Resolve(tenantId));
        if (!resolvedTenantId.HasValue)
        {
            return Results.BadRequest(new { message = "Tenant is required." });
        }

        const string sql = """
            SELECT DISTINCT s.student_id AS "StudentId", s.student_number AS "StudentNumber", s.first_name AS "FirstName", s.last_name AS "LastName", s.status AS "Status", a.class_section_id AS "ClassSectionId" FROM academic.teacher_course_assignment a JOIN student.student_enrollment se ON se.class_section_id = a.class_section_id AND se.tenant_id = a.tenant_id JOIN student.student s ON s.student_id = se.student_id AND s.tenant_id = a.tenant_id WHERE a.tenant_id = @TenantId AND a.employee_id = @EmployeeId;
            """;

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var result = await connection.QueryAsync(new CommandDefinition(
            sql,
            new { TenantId = resolvedTenantId.Value, EmployeeId = employeeId },
            cancellationToken: cancellationToken));

        return Results.Ok(result);
    }
}
