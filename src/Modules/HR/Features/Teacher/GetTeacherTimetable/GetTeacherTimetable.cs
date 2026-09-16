using SmartSchool.Application.Messaging;
using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Teacher.GetTeacherTimetable;

public static class GetTeacherTimetable
{
    public sealed record Request(Guid TenantId, Guid EmployeeId) : IRequest<Response>;
    public sealed record Item(Guid Id, Guid ClassSectionId, int DayOfWeek, string Period, string StartTime, string EndTime, string Subject, string Section, string? Room);
    public sealed record Response(IReadOnlyList<Item> Items);

    public interface IGetTeacherTimetableQuery
    {
        Task<Response> ExecuteAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken);
    }

    internal sealed class GetTeacherTimetableQuery(IDbConnectionFactory connectionFactory, ICurrentUser user) : IGetTeacherTimetableQuery
    {
        public async Task<Response> ExecuteAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT te.timetable_entry_id AS "Id", te.class_section_id AS "ClassSectionId",
                    te.day_of_week::integer AS "DayOfWeek", p.name AS "Period", p.start_time::text AS "StartTime", p.end_time::text AS "EndTime",
                    s.name AS "Subject", cs.name AS "Section", r.name AS "Room"
                FROM academic.teacher_course_assignment a
                JOIN academic.timetable_entry te ON te.teacher_course_assignment_id = a.teacher_course_assignment_id AND te.tenant_id = a.tenant_id
                JOIN academic.timetable_period p ON p.timetable_period_id = te.timetable_period_id AND p.tenant_id = a.tenant_id
                JOIN academic.class_section cs ON cs.class_section_id = te.class_section_id AND cs.tenant_id = a.tenant_id
                JOIN academic.course_offering co ON co.course_offering_id = te.course_offering_id AND co.tenant_id = a.tenant_id
                JOIN academic.subject s ON s.subject_id = co.subject_id AND s.tenant_id = a.tenant_id
                LEFT JOIN org.room r ON r.room_id = te.room_id AND r.tenant_id = a.tenant_id
                WHERE (@ScopeBranchId IS NULL OR cs.campus_id = @ScopeBranchId) AND a.tenant_id = @TenantId AND a.employee_id = @EmployeeId AND a.is_active = TRUE AND te.is_active = TRUE
                    AND (a.effective_from IS NULL OR a.effective_from <= CURRENT_DATE) AND (a.effective_to IS NULL OR a.effective_to >= CURRENT_DATE)
                ORDER BY te.day_of_week, p.start_time;
                """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var rows = await connection.QueryAsync<Item>(new CommandDefinition(sql, new { TenantId = tenantId, EmployeeId = employeeId, ScopeBranchId = user.BranchId }, cancellationToken: cancellationToken));
            return new Response(rows.ToList());
        }
    }

    public sealed class Handler(IGetTeacherTimetableQuery query) : IRequestHandler<Request, Response>
    {
        public Task<Response> HandleAsync(Request request, CancellationToken cancellationToken) => query.ExecuteAsync(request.TenantId, request.EmployeeId, cancellationToken);
    }

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/{employeeId:guid}/timetable", HandleAsync);
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
