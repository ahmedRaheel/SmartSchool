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

namespace SmartSchool.Modules.Transport.Features.Vehicle;

public static class CreateVehicle
{
    public sealed record Request(Guid TenantId, Guid CampusId, string Name, string RegistrationNo, int Capacity) : IRequest<Result<Response>>;
    public sealed record Response(Guid Id, string Name, string RegistrationNo);
    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty(); RuleFor(x => x.CampusId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(250); RuleFor(x => x.RegistrationNo).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Capacity).InclusiveBetween(1, 100);
        }
    }
    public interface ICreateVehicleQuery { Task<bool> CampusExistsAsync(Request request, CancellationToken cancellationToken); }
    internal sealed class CreateVehicleQuery(IDbConnectionFactory factory) : ICreateVehicleQuery
    {
        public async Task<bool> CampusExistsAsync(Request request, CancellationToken cancellationToken)
        {
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("SELECT EXISTS(SELECT 1 FROM org.campus WHERE tenant_id = @TenantId AND campus_id = @CampusId AND is_active)", request, cancellationToken: cancellationToken));
        }
    }
    public interface ICreateVehicleCommand { Task<Result<Response>> AddAsync(VehicleEntity entity, CancellationToken cancellationToken); }
    internal sealed class CreateVehicleCommand(ITransportDbContext db) : ICreateVehicleCommand
    {
        public async Task<Result<Response>> AddAsync(VehicleEntity entity, CancellationToken cancellationToken)
        {
            if (await db.Vehicles.AnyAsync(x => x.TenantId == entity.TenantId && x.RegistrationNo == entity.RegistrationNo && x.IsActive, cancellationToken))
                return Result<Response>.Failure(Error.Conflict("This vehicle registration is already in use."));
            db.Vehicles.Add(entity); await db.SaveChangesAsync(cancellationToken);
            return Result<Response>.Success(new(entity.VehicleId, entity.Name, entity.RegistrationNo));
        }
    }
    public sealed class Handler(ICreateVehicleQuery query, ICreateVehicleCommand command, IBusinessNumberGenerator numbers) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            if (!await query.CampusExistsAsync(request, cancellationToken)) return Result<Response>.Failure(Error.Validation("Select an active campus."));
            var code = await numbers.NextAsync("VEHICLE", "VEH-", request.TenantId, 6, cancellationToken);
            return await command.AddAsync(VehicleEntity.Register(request.TenantId, request.CampusId, code, request.Name, request.RegistrationNo, request.Capacity), cancellationToken);
        }
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/transport/vehicle", async (Request request, ITenantScope scope, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var tenant = scope.Resolve(request.TenantId);
            if (!tenant.HasValue) return Results.BadRequest(new { message = "Select a tenant." });
            return (await mediator.SendAsync<Request, Result<Response>>(request with { TenantId = tenant.Value }, cancellationToken)).ToHttpResult();
        }).WithName("CreateVehicle").WithTags("Transport").RequireAuthorization(SmartSchoolPolicies.SchoolAdministration);
        return endpoints;
    }
}
