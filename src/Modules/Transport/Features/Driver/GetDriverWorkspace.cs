using Dapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Transport.Authorization;
using SmartSchool.Modules.Transport.Models;
using SmartSchool.Modules.Transport.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Transport.Features.Driver;

public static class GetDriverWorkspace
{
    public sealed record Query(Guid TenantId, DateOnly ServiceDate, string Direction, Guid? DriverId) : IRequest<Result<Response>>;
    public sealed record Route(Guid Id, string Name, string DriverName, string Vehicle, int? Capacity,
        string? StartTime, string? ArrivalTime, string? DismissalTime);
    public sealed record Student(Guid StudentId, Guid RouteId, Guid StopId, string Name, string Number,
        string Stop, int Sequence, string? PickupTime, string? DropoffTime, string Status);
    public sealed record Notice(Guid Id, Guid RouteId, int DelayMinutes, string Message, DateTimeOffset CreatedAt);
    public sealed record Response(IReadOnlyList<Route> Routes, IReadOnlyList<Student> Students, IReadOnlyList<Notice> Notices);
    public interface IGetDriverWorkspaceQuery { Task<Response> GetAsync(Query request, CancellationToken cancellationToken); }
    internal sealed class GetDriverWorkspaceQuery(IDbConnectionFactory factory, ICurrentUser user) : IGetDriverWorkspaceQuery
    {
        public async Task<Response> GetAsync(Query request, CancellationToken cancellationToken)
        {
            const string scopeSql = """
                r.tenant_id = @TenantId AND r.is_active AND d.is_active AND e.is_active
                AND (@CampusId IS NULL OR r.campus_id = @CampusId)
                AND ((@Manage AND (@RequestedDriverId IS NULL OR d.driver_id = @RequestedDriverId))
                    OR (NOT @Manage AND (d.driver_id = @DriverId OR d.employee_id = @EmployeeId OR e.user_id = @UserId)))
                """;
            const string joins = """
                FROM transport.route r JOIN transport.driver d ON d.driver_id = r.driver_id AND d.tenant_id = r.tenant_id
                JOIN hr.employee e ON e.employee_id = d.employee_id AND e.tenant_id = d.tenant_id
                """;
            var routesSql = """
                SELECT r.route_id AS "Id", r.name AS "Name", d.full_name AS "DriverName",
                    v.registration_no AS "Vehicle", v.capacity AS "Capacity", r.start_time::text AS "StartTime",
                    r.arrival_time::text AS "ArrivalTime", r.dismissal_time::text AS "DismissalTime"
                """ + " " + joins + " " + " JOIN transport.vehicle v ON v.vehicle_id = r.vehicle_id AND v.tenant_id = r.tenant_id WHERE " + scopeSql + " ORDER BY r.name;";
            var studentsSql = """
                SELECT s.student_id AS "StudentId", r.route_id AS "RouteId", st.stop_id AS "StopId",
                    trim(s.first_name || ' ' || coalesce(s.last_name, '')) AS "Name", coalesce(s.student_number, '') AS "Number",
                    st.name AS "Stop", st.sequence AS "Sequence", st.pickup_time::text AS "PickupTime", st.dropoff_time::text AS "DropoffTime",
                    coalesce(tr.status, 'WAITING') AS "Status"
                """ + " " + joins + " " + """
                JOIN transport.studenttransport a ON a.route_id = r.route_id AND a.tenant_id = r.tenant_id AND a.is_active
                JOIN student.student s ON s.student_id = a.student_id AND s.tenant_id = a.tenant_id AND s.is_active
                JOIN transport.stop st ON st.stop_id = a.stop_id AND st.tenant_id = a.tenant_id AND st.route_id = r.route_id AND st.is_active
                LEFT JOIN transport.trip_record tr ON tr.tenant_id = r.tenant_id AND tr.route_id = r.route_id
                    AND tr.student_id = s.student_id AND tr.service_date = @ServiceDate AND tr.direction = @Direction AND tr.is_active
                WHERE
                """ + " " + scopeSql + " ORDER BY st.sequence, s.first_name, s.last_name;";
            var noticesSql = """
                SELECT n.route_notice_id AS "Id", r.route_id AS "RouteId", n.delay_minutes AS "DelayMinutes", n.message AS "Message", n.created_at AS "CreatedAt"
                """ + " " + joins + " " + " JOIN transport.route_notice n ON n.route_id = r.route_id AND n.tenant_id = r.tenant_id AND n.service_date = @ServiceDate AND n.is_active WHERE " + scopeSql + " ORDER BY n.created_at DESC;";
            var parameters = new { request.TenantId, request.ServiceDate, request.Direction, RequestedDriverId = request.DriverId,
                CampusId = user.BranchId, Manage = TransportPermissions.CanManage(user), user.DriverId, user.EmployeeId, user.UserId };
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            var routes = (await connection.QueryAsync<Route>(new CommandDefinition(routesSql, parameters, cancellationToken: cancellationToken))).AsList();
            var students = (await connection.QueryAsync<Student>(new CommandDefinition(studentsSql, parameters, cancellationToken: cancellationToken))).AsList();
            var notices = (await connection.QueryAsync<Notice>(new CommandDefinition(noticesSql, parameters, cancellationToken: cancellationToken))).AsList();
            return new(routes, students, notices);
        }
    }
    public sealed class Handler(IGetDriverWorkspaceQuery query) : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Query request, CancellationToken cancellationToken) => Result<Response>.Success(await query.GetAsync(request, cancellationToken));
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/transport/driver/workspace", async (Guid? tenantId, DateOnly? serviceDate, string? direction,
            Guid? driverId, ITenantScope scope, ICurrentUser user, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!TransportPermissions.CanManage(user) && !user.IsInRole(SmartSchoolRoles.Driver)) return Results.Forbid();
            var tenant = scope.Resolve(tenantId);
            if (!tenant.HasValue) return Results.BadRequest(new { message = "Select a tenant." });
            var tripDirection = direction?.ToUpperInvariant() ?? "PICKUP";
            if (tripDirection is not ("PICKUP" or "DROPOFF")) return Results.BadRequest(new { message = "Direction must be PICKUP or DROPOFF." });
            return (await mediator.SendAsync<Query, Result<Response>>(new(tenant.Value, serviceDate ?? DateOnly.FromDateTime(DateTime.UtcNow), tripDirection, driverId), cancellationToken)).ToHttpResult();
        }).WithName("GetDriverWorkspace").WithTags("Transport").RequireAuthorization();
        return endpoints;
    }
}
