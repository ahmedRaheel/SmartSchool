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

public static class CreateRouteNotice
{
    public sealed record Request(Guid TenantId, Guid RouteId, DateOnly ServiceDate, int DelayMinutes, string Message) : IRequest<Result<Response>>;
    public sealed record Response(Guid Id, string Message);
    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty(); RuleFor(x => x.RouteId).NotEmpty();
            RuleFor(x => x.DelayMinutes).InclusiveBetween(0, 360); RuleFor(x => x.Message).NotEmpty().MaximumLength(2000);
            RuleFor(x => x.ServiceDate).Must(x => x >= DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1) && x <= DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1));
        }
    }
    public interface ICreateRouteNoticeQuery { Task<bool> CanWriteAsync(Request request, CancellationToken cancellationToken); }
    internal sealed class CreateRouteNoticeQuery(IDbConnectionFactory factory, ICurrentUser user) : ICreateRouteNoticeQuery
    {
        public async Task<bool> CanWriteAsync(Request request, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT EXISTS (SELECT 1 FROM transport.route r
                    JOIN transport.driver d ON d.driver_id = r.driver_id AND d.tenant_id = r.tenant_id AND d.is_active
                    JOIN hr.employee e ON e.employee_id = d.employee_id AND e.tenant_id = d.tenant_id AND e.is_active
                    WHERE r.tenant_id = @TenantId AND r.route_id = @RouteId AND r.is_active
                        AND (@CampusId IS NULL OR r.campus_id = @CampusId)
                        AND (@Manage OR d.driver_id = @DriverId OR d.employee_id = @EmployeeId OR e.user_id = @UserId));
                """;
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { request.TenantId, request.RouteId,
                CampusId = user.BranchId, Manage = TransportPermissions.CanManage(user), user.DriverId, user.EmployeeId, user.UserId }, cancellationToken: cancellationToken));
        }
    }
    public interface ICreateRouteNoticeCommand { Task<Response> SaveAsync(Request request, CancellationToken cancellationToken); }
    internal sealed class CreateRouteNoticeCommand(ITransportDbContext db, ICurrentUser user) : ICreateRouteNoticeCommand
    {
        public async Task<Response> SaveAsync(Request request, CancellationToken cancellationToken)
        {
            var entity = RouteNoticeEntity.Create(request.TenantId, request.RouteId, request.ServiceDate, request.DelayMinutes, request.Message, user.UserId);
            db.RouteNotices.Add(entity); await db.SaveChangesAsync(cancellationToken);
            return new(entity.RouteNoticeId, entity.Message);
        }
    }
    public sealed class Handler(ICreateRouteNoticeQuery query, ICreateRouteNoticeCommand command) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken) =>
            await query.CanWriteAsync(request, cancellationToken) ? Result<Response>.Success(await command.SaveAsync(request, cancellationToken))
                : Result<Response>.Failure(Error.Forbidden("You cannot add a notice to this route."));
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/transport/driver/notices", async (Request request, ITenantScope scope, ICurrentUser user, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!TransportPermissions.CanManage(user) && !user.IsInRole(SmartSchoolRoles.Driver)) return Results.Forbid();
            var tenant = scope.Resolve(request.TenantId);
            if (!tenant.HasValue) return Results.BadRequest(new { message = "Select a tenant." });
            return (await mediator.SendAsync<Request, Result<Response>>(request with { TenantId = tenant.Value }, cancellationToken)).ToHttpResult();
        }).WithName("CreateRouteNotice").WithTags("Transport").RequireAuthorization();
        return endpoints;
    }
}
