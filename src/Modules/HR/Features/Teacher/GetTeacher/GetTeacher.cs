using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Teacher.GetTeacher;

public static class GetTeacher
{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/{employeeId:guid}", HandleAsync);
    }

    private static async Task<IResult> HandleAsync(Guid employeeId, Guid? tenantId, ITenantScope tenantScope, IDbConnectionFactory connectionFactory, CancellationToken cancellationToken)
    {
        var resolvedTenantId = tenantScope.IsSuperAdmin ? tenantId : tenantScope.Resolve(tenantId);
        if (!resolvedTenantId.HasValue)
        {
            return Results.BadRequest(new { message = "Tenant is required." });
        }

        const string sql = """
            SELECT e.employee_id AS "EmployeeId", e.tenant_id AS "TenantId", e.user_id AS "UserId", e.employee_number AS "EmployeeNumber", e.first_name AS "FirstName", e.last_name AS "LastName", e.email AS "Email", e.phone AS "Phone", e.hire_date AS "HireDate", e.employment_type_code AS "EmploymentType", e.status AS "Status", tp.qualification AS "Qualification", tp.specialization AS "Specialization", tp.teaching_experience_years AS "TeachingExperienceYears" FROM hr.employee e LEFT JOIN hr."TeacherProfile" tp ON tp."EmployeeId" = e.employee_id AND tp."TenantId" = e.tenant_id WHERE e.tenant_id = @TenantId AND e.employee_id = @EmployeeId;
            """;

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var result = await connection.QueryAsync(new CommandDefinition(
            sql,
            new { TenantId = resolvedTenantId.Value, EmployeeId = employeeId },
            cancellationToken: cancellationToken));

        return Results.Ok(result);
    }
}
