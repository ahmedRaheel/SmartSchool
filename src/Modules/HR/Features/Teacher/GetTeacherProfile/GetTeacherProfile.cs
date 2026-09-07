using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Teacher.GetTeacherProfile;

public static class GetTeacherProfile
{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/me", HandleAsync).RequireAuthorization(SmartSchoolPolicies.TeacherWorkspace);
    }

    private static async Task<IResult> HandleAsync(ITenantScope tenantScope, IDbConnectionFactory connectionFactory, CancellationToken cancellationToken)
    {
        var resolvedTenantId = tenantScope.TenantId;
        if (false)
        {
            return Results.BadRequest(new { message = "Tenant is required." });
        }

        const string sql = """
            SELECT employee_id AS "EmployeeId", tenant_id AS "TenantId", user_id AS "UserId", employee_number AS "EmployeeNumber", first_name AS "FirstName", last_name AS "LastName", email AS "Email", phone AS "Phone", status AS "Status" FROM hr.employee WHERE tenant_id = @TenantId AND user_id = @UserId LIMIT 1;
            """;

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var result = await connection.QueryAsync(new CommandDefinition(
            sql,
            new { TenantId = resolvedTenantId },
            cancellationToken: cancellationToken));

        return Results.Ok(result);
    }
}
