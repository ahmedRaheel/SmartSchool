using Dapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Transport.Models;
using SmartSchool.Modules.Transport.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Transport.Features.Operations;

public static class GetTransportOperations
{
    public sealed record Query(Guid TenantId) : IRequest<Result<Response>>;
    public sealed record Lookup(Guid Id, string Name, Guid? CampusId);
    public sealed record Vehicle(Guid Id, string Name, Guid CampusId, string RegistrationNo, int? Capacity);
    public sealed record Route(Guid Id, string Name, Guid CampusId, Guid? VehicleId, Guid? DriverId,
        string? StartTime, string? ArrivalTime, string? DismissalTime, long StudentCount);
    public sealed record Stop(Guid Id, Guid? RouteId, string Name, int Sequence, string? PickupTime, string? DropoffTime);
    public sealed record Assignment(Guid Id, Guid? StudentId, string StudentName, Guid? RouteId, Guid? StopId);
    public sealed record Notice(Guid Id, Guid RouteId, DateOnly ServiceDate, int DelayMinutes, string Message, DateTimeOffset CreatedAt);
    public sealed record Response(IReadOnlyList<Lookup> Campuses, IReadOnlyList<Lookup> Drivers,
        IReadOnlyList<Lookup> Students, IReadOnlyList<Lookup> DriverEmployees, IReadOnlyList<Vehicle> Vehicles,
        IReadOnlyList<Route> Routes, IReadOnlyList<Stop> Stops, IReadOnlyList<Assignment> Assignments, IReadOnlyList<Notice> Notices);
    public interface IGetTransportOperationsQuery { Task<Response> GetAsync(Query request, CancellationToken cancellationToken); }
    internal sealed class GetTransportOperationsQuery(IDbConnectionFactory factory, ICurrentUser user) : IGetTransportOperationsQuery
    {
        public async Task<Response> GetAsync(Query request, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT campus_id AS "Id", name AS "Name", campus_id AS "CampusId" FROM org.campus
                WHERE tenant_id = @TenantId AND is_active AND (@CampusId IS NULL OR campus_id = @CampusId) ORDER BY name;
                SELECT d.driver_id AS "Id", d.full_name AS "Name", e.branch_id AS "CampusId"
                FROM transport.driver d JOIN hr.employee e ON e.employee_id = d.employee_id AND e.tenant_id = d.tenant_id
                WHERE d.tenant_id = @TenantId AND d.is_active AND d.status = 'ACTIVE' AND e.is_active
                    AND (@CampusId IS NULL OR e.branch_id = @CampusId) ORDER BY d.full_name;
                SELECT student_id AS "Id", trim(first_name || ' ' || coalesce(last_name, '')) AS "Name", branch_id AS "CampusId"
                FROM student.student WHERE tenant_id = @TenantId AND is_active AND status = 'ACTIVE'
                    AND (@CampusId IS NULL OR branch_id = @CampusId) ORDER BY first_name, last_name;
                SELECT e.employee_id AS "Id", trim(e.first_name || ' ' || coalesce(e.last_name, '')) AS "Name", e.branch_id AS "CampusId"
                FROM hr.employee e WHERE e.tenant_id = @TenantId AND e.is_active AND upper(e.staff_type) = 'DRIVER'
                    AND e.status IN ('ACTIVE', 'APPROVED') AND (@CampusId IS NULL OR e.branch_id = @CampusId)
                    AND NOT EXISTS (SELECT 1 FROM transport.driver d WHERE d.tenant_id = e.tenant_id AND d.employee_id = e.employee_id AND d.is_active)
                ORDER BY e.first_name;
                SELECT vehicle_id AS "Id", name AS "Name", campus_id AS "CampusId", registration_no AS "RegistrationNo", capacity AS "Capacity"
                FROM transport.vehicle WHERE tenant_id = @TenantId AND is_active AND status = 'ACTIVE'
                    AND (@CampusId IS NULL OR campus_id = @CampusId) ORDER BY name;
                SELECT r.route_id AS "Id", r.name AS "Name", r.campus_id AS "CampusId", r.vehicle_id AS "VehicleId", r.driver_id AS "DriverId",
                    r.start_time::text AS "StartTime", r.arrival_time::text AS "ArrivalTime", r.dismissal_time::text AS "DismissalTime",
                    (SELECT count(*) FROM transport.studenttransport st WHERE st.tenant_id = r.tenant_id AND st.route_id = r.route_id AND st.is_active) AS "StudentCount"
                FROM transport.route r WHERE r.tenant_id = @TenantId AND r.is_active AND (@CampusId IS NULL OR r.campus_id = @CampusId) ORDER BY r.name;
                SELECT s.stop_id AS "Id", s.route_id AS "RouteId", s.name AS "Name", s.sequence AS "Sequence", s.pickup_time::text AS "PickupTime", s.dropoff_time::text AS "DropoffTime"
                FROM transport.stop s JOIN transport.route r ON r.route_id = s.route_id AND r.tenant_id = s.tenant_id
                WHERE s.tenant_id = @TenantId AND s.is_active AND r.is_active AND (@CampusId IS NULL OR r.campus_id = @CampusId) ORDER BY s.sequence;
                SELECT st.student_transport_id AS "Id", st.student_id AS "StudentId", trim(s.first_name || ' ' || coalesce(s.last_name, '')) AS "StudentName",
                    st.route_id AS "RouteId", st.stop_id AS "StopId" FROM transport.studenttransport st
                JOIN transport.route r ON r.route_id = st.route_id AND r.tenant_id = st.tenant_id
                JOIN student.student s ON s.student_id = st.student_id AND s.tenant_id = st.tenant_id
                WHERE st.tenant_id = @TenantId AND st.is_active AND r.is_active AND (@CampusId IS NULL OR r.campus_id = @CampusId) ORDER BY s.first_name;
                SELECT n.route_notice_id AS "Id", n.route_id AS "RouteId", n.service_date AS "ServiceDate", n.delay_minutes AS "DelayMinutes", n.message AS "Message", n.created_at AS "CreatedAt"
                FROM transport.route_notice n JOIN transport.route r ON r.route_id = n.route_id AND r.tenant_id = n.tenant_id
                WHERE n.tenant_id = @TenantId AND n.is_active AND (@CampusId IS NULL OR r.campus_id = @CampusId)
                    AND n.service_date >= CURRENT_DATE - 7 ORDER BY n.created_at DESC LIMIT 100;
                """;
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            using var grid = await connection.QueryMultipleAsync(new CommandDefinition(sql, new { request.TenantId, CampusId = user.BranchId }, cancellationToken: cancellationToken));
            return new((await grid.ReadAsync<Lookup>()).AsList(), (await grid.ReadAsync<Lookup>()).AsList(),
                (await grid.ReadAsync<Lookup>()).AsList(), (await grid.ReadAsync<Lookup>()).AsList(),
                (await grid.ReadAsync<Vehicle>()).AsList(), (await grid.ReadAsync<Route>()).AsList(),
                (await grid.ReadAsync<Stop>()).AsList(), (await grid.ReadAsync<Assignment>()).AsList(), (await grid.ReadAsync<Notice>()).AsList());
        }
    }
    public sealed class Handler(IGetTransportOperationsQuery query) : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Query request, CancellationToken cancellationToken) => Result<Response>.Success(await query.GetAsync(request, cancellationToken));
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/transport/operations", async (Guid? tenantId, ITenantScope scope, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var tenant = scope.Resolve(tenantId);
            if (!tenant.HasValue) return Results.BadRequest(new { message = "Select a tenant." });
            return (await mediator.SendAsync<Query, Result<Response>>(new(tenant.Value), cancellationToken)).ToHttpResult();
        }).WithName("GetTransportOperations").WithTags("Transport").RequireAuthorization(SmartSchoolPolicies.SchoolAdministration);
        return endpoints;
    }
}
