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

public static class RecordTripStatus
{
    public sealed record Request(Guid TenantId, Guid RouteId, Guid StudentId, DateOnly ServiceDate,
        string Direction, string Status) : IRequest<Result<Response>>;
    public sealed record Response(Guid Id, string Status);
    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty(); RuleFor(x => x.RouteId).NotEmpty(); RuleFor(x => x.StudentId).NotEmpty();
            RuleFor(x => x.Direction).Must(x => x is "PICKUP" or "DROPOFF");
            RuleFor(x => x.Status).Must(x => x is "WAITING" or "BOARDED" or "DROPPED_OFF" or "ABSENT");
            RuleFor(x => x.ServiceDate).Must(x => x >= DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-7) && x <= DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1))
                .WithMessage("Trip records can be recorded for the last seven days or today's service.");
        }
    }
    public interface IRecordTripStatusQuery { Task<Guid?> GetDriverAsync(Request request, CancellationToken cancellationToken); }
    internal sealed class RecordTripStatusQuery(IDbConnectionFactory factory, ICurrentUser user) : IRecordTripStatusQuery
    {
        public async Task<Guid?> GetDriverAsync(Request request, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT r.driver_id FROM transport.route r
                JOIN transport.driver d ON d.driver_id = r.driver_id AND d.tenant_id = r.tenant_id AND d.is_active
                JOIN hr.employee e ON e.employee_id = d.employee_id AND e.tenant_id = d.tenant_id AND e.is_active
                WHERE r.tenant_id = @TenantId AND r.route_id = @RouteId AND r.is_active
                    AND (@CampusId IS NULL OR r.campus_id = @CampusId)
                    AND (@Manage OR d.driver_id = @DriverId OR d.employee_id = @EmployeeId OR e.user_id = @UserId)
                    AND EXISTS (SELECT 1 FROM transport.studenttransport a WHERE a.tenant_id = r.tenant_id AND a.route_id = r.route_id AND a.student_id = @StudentId AND a.is_active);
                """;
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<Guid?>(new CommandDefinition(sql, new { request.TenantId, request.RouteId, request.StudentId,
                CampusId = user.BranchId, Manage = TransportPermissions.CanManage(user), user.DriverId, user.EmployeeId, user.UserId }, cancellationToken: cancellationToken));
        }
    }
    public interface IRecordTripStatusCommand { Task<Result<Response>> SaveAsync(Request request, Guid driverId, CancellationToken cancellationToken); }
    internal sealed class RecordTripStatusCommand(ITransportDbContext db, ICurrentUser user) : IRecordTripStatusCommand
    {
        public async Task<Result<Response>> SaveAsync(Request request, Guid driverId, CancellationToken cancellationToken)
        {
            await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
            var route = await db.Routes.FromSqlInterpolated($"SELECT * FROM transport.route WHERE tenant_id = {request.TenantId} AND route_id = {request.RouteId} FOR UPDATE").SingleAsync(cancellationToken);
            if (route.DriverId != driverId) return Result<Response>.Failure(Error.Conflict("The route assignment changed. Reload your route."));
            var record = await db.TripRecords.SingleOrDefaultAsync(x => x.TenantId == request.TenantId && x.RouteId == request.RouteId &&
                x.StudentId == request.StudentId && x.ServiceDate == request.ServiceDate && x.Direction == request.Direction, cancellationToken);
            if (request.Status == "DROPPED_OFF" && record?.Status is not ("BOARDED" or "DROPPED_OFF"))
                return Result<Response>.Failure(Error.Validation("Record boarding before drop-off."));
            if (record is null) { record = TripRecordEntity.Record(request.TenantId, request.RouteId, request.StudentId, request.ServiceDate, request.Direction, request.Status, user.UserId); db.TripRecords.Add(record); }
            else record.SetStatus(request.Status, user.UserId);
            await db.SaveChangesAsync(cancellationToken); await transaction.CommitAsync(cancellationToken);
            return Result<Response>.Success(new(record.TripRecordId, record.Status));
        }
    }
    public sealed class Handler(IRecordTripStatusQuery query, IRecordTripStatusCommand command) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var driverId = await query.GetDriverAsync(request, cancellationToken);
            return driverId.HasValue ? await command.SaveAsync(request, driverId.Value, cancellationToken)
                : Result<Response>.Failure(Error.Forbidden("This student is not on a route assigned to you."));
        }
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/api/transport/driver/trip-status", async (Request request, ITenantScope scope, ICurrentUser user, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!TransportPermissions.CanManage(user) && !user.IsInRole(SmartSchoolRoles.Driver)) return Results.Forbid();
            var tenant = scope.Resolve(request.TenantId);
            if (!tenant.HasValue) return Results.BadRequest(new { message = "Select a tenant." });
            return (await mediator.SendAsync<Request, Result<Response>>(request with { TenantId = tenant.Value }, cancellationToken)).ToHttpResult();
        }).WithName("RecordTripStatus").WithTags("Transport").RequireAuthorization();
        return endpoints;
    }
}
