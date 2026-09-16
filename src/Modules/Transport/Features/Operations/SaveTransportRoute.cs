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

public static class SaveTransportRoute
{
    public sealed record Stop(Guid? Id, string Name, TimeOnly PickupTime, TimeOnly DropoffTime);
    public sealed record Request(Guid TenantId, Guid? Id, string Name, Guid CampusId, Guid VehicleId, Guid DriverId,
        TimeOnly StartTime, TimeOnly ArrivalTime, TimeOnly DismissalTime, IReadOnlyList<Stop> Stops) : IRequest<Result<Response>>;
    public sealed record Response(Guid Id, string Name);
    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty(); RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
            RuleFor(x => x.CampusId).NotEmpty(); RuleFor(x => x.VehicleId).NotEmpty(); RuleFor(x => x.DriverId).NotEmpty();
            RuleFor(x => x.ArrivalTime).GreaterThan(x => x.StartTime);
            RuleFor(x => x.DismissalTime).GreaterThan(x => x.ArrivalTime);
            RuleFor(x => x.Stops).NotEmpty().Must(x => x is not null && x.Count <= 100);
            RuleForEach(x => x.Stops).ChildRules(v => v.RuleFor(x => x.Name).NotEmpty().MaximumLength(250));
            RuleFor(x => x).Must(x => x.Stops is not null && x.Stops.All(s => s.PickupTime >= x.StartTime && s.PickupTime <= x.ArrivalTime && s.DropoffTime >= x.DismissalTime))
                .WithMessage("Stop times must fit the pickup and dismissal schedule.");
            RuleFor(x => x.Stops).Must(x => x is not null && x.Where(s => s.Id.HasValue).Select(s => s.Id).Distinct().Count() == x.Count(s => s.Id.HasValue));
        }
    }
    public interface ISaveTransportRouteQuery { Task<bool> ValidateAsync(Request request, CancellationToken cancellationToken); }
    internal sealed class SaveTransportRouteQuery(IDbConnectionFactory factory, ICurrentUser user) : ISaveTransportRouteQuery
    {
        public async Task<bool> ValidateAsync(Request request, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT EXISTS (SELECT 1 FROM transport.vehicle v
                    JOIN org.campus c ON c.campus_id = v.campus_id AND c.tenant_id = v.tenant_id AND c.is_active
                    JOIN hr.employee e ON e.tenant_id = c.tenant_id AND e.branch_id = c.campus_id AND e.is_active
                    JOIN transport.driver d ON d.employee_id = e.employee_id AND d.tenant_id = e.tenant_id AND d.is_active
                    WHERE v.tenant_id = @TenantId AND v.vehicle_id = @VehicleId AND v.campus_id = @CampusId
                        AND v.is_active AND v.status = 'ACTIVE' AND d.driver_id = @DriverId AND d.status = 'ACTIVE'
                        AND coalesce(d.driving_license_expires_on, d.license_expiry_date) >= CURRENT_DATE);
                """;
            if (user.BranchId.HasValue && user.BranchId != request.CampusId) return false;
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, request, cancellationToken: cancellationToken));
        }
    }
    public interface ISaveTransportRouteCommand { Task<Result<Response>> SaveAsync(Request request, CancellationToken cancellationToken); }
    internal sealed class SaveTransportRouteCommand(ITransportDbContext db, IBusinessNumberGenerator numbers) : ISaveTransportRouteCommand
    {
        public async Task<Result<Response>> SaveAsync(Request request, CancellationToken cancellationToken)
        {
            var code = await numbers.NextAsync("TRANSPORT_ROUTE", "RTE-", request.TenantId, 6, cancellationToken);
            await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
            RouteEntity? route;
            if (request.Id.HasValue)
            {
                route = await db.Routes.FromSqlInterpolated($"SELECT * FROM transport.route WHERE tenant_id = {request.TenantId} AND route_id = {request.Id.Value} FOR UPDATE").SingleOrDefaultAsync(cancellationToken);
                if (route is null || !route.IsActive || route.CampusId != request.CampusId) return Result<Response>.Failure(Error.NotFound("Route not found in this campus."));
            }
            else
            {
                
                route = RouteEntity.Schedule(request.TenantId, code, request.Name, request.CampusId, request.VehicleId, request.DriverId, request.StartTime, request.ArrivalTime, request.DismissalTime);
                db.Routes.Add(route);
            }
            var capacity = await db.Vehicles.Where(x => x.TenantId == request.TenantId && x.VehicleId == request.VehicleId).Select(x => x.Capacity).SingleAsync(cancellationToken);
            var assigned = await db.StudentTransports.CountAsync(x => x.TenantId == request.TenantId && x.RouteId == route.RouteId && x.IsActive, cancellationToken);
            if (assigned > capacity) return Result<Response>.Failure(Error.Validation("The selected vehicle cannot seat the assigned students."));
            var stops = await db.Stops.Where(x => x.TenantId == request.TenantId && x.RouteId == route.RouteId && x.IsActive).ToListAsync(cancellationToken);
            if (request.Stops.Any(x => x.Id.HasValue && stops.All(s => s.StopId != x.Id)))
                return Result<Response>.Failure(Error.Validation("A stop does not belong to this route."));
            var removed = stops.Where(x => request.Stops.All(s => s.Id != x.StopId)).ToList();
            var removedIds = removed.Select(x => x.StopId).ToArray();
            if (await db.StudentTransports.AnyAsync(x => x.TenantId == request.TenantId && x.IsActive && x.StopId.HasValue && removedIds.Contains(x.StopId.Value), cancellationToken))
                return Result<Response>.Failure(Error.Conflict("Move students to another stop before removing their current stop."));
            foreach (var stop in removed) stop.Deactivate();
            for (var i = 0; i < request.Stops.Count; i++)
            {
                var draft = request.Stops[i];
                if (draft.Id.HasValue) stops.Single(x => x.StopId == draft.Id).Configure(draft.Name, i + 1, draft.PickupTime, draft.DropoffTime);
                else db.Stops.Add(StopEntity.Add(request.TenantId, route.RouteId, draft.Name, i + 1, draft.PickupTime, draft.DropoffTime));
            }
            route.Configure(request.Name, request.CampusId, request.VehicleId, request.DriverId, request.StartTime, request.ArrivalTime, request.DismissalTime);
            await db.SaveChangesAsync(cancellationToken); await transaction.CommitAsync(cancellationToken);
            return Result<Response>.Success(new(route.RouteId, route.Name));
        }
    }
    public sealed class Handler(ISaveTransportRouteQuery query, ISaveTransportRouteCommand command) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken) =>
            await query.ValidateAsync(request, cancellationToken) ? await command.SaveAsync(request, cancellationToken)
                : Result<Response>.Failure(Error.Validation("Choose an active vehicle and a driver with a valid licence in the route's campus."));
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/transport/operations/routes", async (Request request, ITenantScope scope, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var tenant = scope.Resolve(request.TenantId);
            if (!tenant.HasValue) return Results.BadRequest(new { message = "Select a tenant." });
            return (await mediator.SendAsync<Request, Result<Response>>(request with { TenantId = tenant.Value }, cancellationToken)).ToHttpResult();
        }).WithName("SaveTransportRoute").WithTags("Transport").RequireAuthorization(SmartSchoolPolicies.SchoolAdministration);
        return endpoints;
    }
}
