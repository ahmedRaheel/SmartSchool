using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Teacher.GetTeacherClasses;

public static class GetTeacherClasses
{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/{employeeId:guid}/classes", HandleAsync);
    }

    private static async Task<IResult> HandleAsync(Guid employeeId, Guid? tenantId, ITenantScope tenantScope, IDbConnectionFactory connectionFactory, CancellationToken cancellationToken)
    {
        var resolvedTenantId = tenantScope.IsSuperAdmin ? tenantId : tenantScope.Resolve(tenantId);
        if (!resolvedTenantId.HasValue)
        {
            return Results.BadRequest(new { message = "Tenant is required." });
        }

        const string sql = """
            SELECT a.teacher_course_assignment_id AS "AssignmentId", a.course_offering_id AS "CourseOfferingId", a.class_section_id AS "ClassSectionId", a.assignment_role AS "Role", a.periods_per_week AS "PeriodsPerWeek", a.effective_from AS "EffectiveFrom", a.effective_to AS "EffectiveTo" FROM academic.teacher_course_assignment a WHERE a.tenant_id = @TenantId AND a.employee_id = @EmployeeId AND (a.effective_to IS NULL OR a.effective_to >= CURRENT_DATE);
            """;

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var result = await connection.QueryAsync(new CommandDefinition(
            sql,
            new { TenantId = resolvedTenantId.Value, EmployeeId = employeeId },
            cancellationToken: cancellationToken));

        return Results.Ok(result);
    }
}
