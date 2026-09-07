using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Teacher.GetTeacherTimetable;

public static class GetTeacherTimetable
{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/{employeeId:guid}/timetable", HandleAsync);
    }

    private static async Task<IResult> HandleAsync(Guid employeeId, Guid? tenantId, ITenantScope tenantScope, IDbConnectionFactory connectionFactory, CancellationToken cancellationToken)
    {
        var resolvedTenantId = tenantScope.IsSuperAdmin ? tenantId : tenantScope.Resolve(tenantId);
        if (!resolvedTenantId.HasValue)
        {
            return Results.BadRequest(new { message = "Tenant is required." });
        }

        const string sql = """
            SELECT te.timetable_entry_id AS "TimetableEntryId", te.day_of_week AS "DayOfWeek", p.name AS "Period", p.start_time AS "StartTime", p.end_time AS "EndTime", te.class_section_id AS "ClassSectionId", te.course_offering_id AS "CourseOfferingId", te.room_id AS "RoomId" FROM academic.teacher_course_assignment a JOIN academic.timetable_entry te ON te.teacher_course_assignment_id = a.teacher_course_assignment_id JOIN academic.timetable_period p ON p.timetable_period_id = te.timetable_period_id WHERE a.tenant_id = @TenantId AND a.employee_id = @EmployeeId ORDER BY te.day_of_week, p.start_time;
            """;

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var result = await connection.QueryAsync(new CommandDefinition(
            sql,
            new { TenantId = resolvedTenantId.Value, EmployeeId = employeeId },
            cancellationToken: cancellationToken));

        return Results.Ok(result);
    }
}
