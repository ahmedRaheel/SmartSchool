using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Teacher.GetTeacherWorkload;

public static class GetTeacherWorkload
{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/{employeeId:guid}/workload", HandleAsync);
    }

    private static async Task<IResult> HandleAsync(Guid employeeId, Guid? tenantId, ITenantScope tenantScope, IDbConnectionFactory connectionFactory, CancellationToken cancellationToken)
    {
        var resolvedTenantId = (tenantScope.IsSuperAdmin ? tenantId : tenantScope.Resolve(tenantId));
        if (!resolvedTenantId.HasValue)
        {
            return Results.BadRequest(new { message = "Tenant is required." });
        }

        const string sql = """
            SELECT @EmployeeId AS "EmployeeId", COUNT(*) AS "ActiveAssignments", COALESCE(SUM(periods_per_week), 0) AS "PeriodsPerWeek", COUNT(DISTINCT class_section_id) AS "Classes" FROM academic.teacher_course_assignment WHERE tenant_id = @TenantId AND employee_id = @EmployeeId AND (effective_to IS NULL OR effective_to >= CURRENT_DATE);
            """;

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var result = await connection.QueryAsync(new CommandDefinition(
            sql,
            new { TenantId = resolvedTenantId.Value, EmployeeId = employeeId },
            cancellationToken: cancellationToken));

        return Results.Ok(result);
    }
}
