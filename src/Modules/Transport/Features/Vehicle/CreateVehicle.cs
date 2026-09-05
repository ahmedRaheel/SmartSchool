using SmartSchool.Modules.Transport.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Transport.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Transport.Features.Vehicle;

public static class CreateVehicle
{
    /// <summary>
    /// Represents the response returned by this VehicleEntity feature.
    /// </summary>
    /// <param name="TenantId">The owning tenant identifier.</param>
    /// <param name="Id">The entity identifier.</param>
    /// <param name="Code">The business code.</param>
    /// <param name="Name">The display name.</param>
    public sealed record Response(
    Guid TenantId,
    Guid Id,
    string Code,
    string Name,
    string? MetadataJson);

    public sealed record Request(
        Guid TenantId,
        string Name) : IRequest<Result<Response>>;

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        }
    }

    public interface ICreateVehicle
    {
        Task AddAsync(
                VehicleEntity entity,
                CancellationToken cancellationToken);
}

    internal sealed class CreateVehiclePersistence(ITransportDbContext dbContext) : ICreateVehicle
    {
        public async Task AddAsync(
                VehicleEntity entity,
                CancellationToken cancellationToken)
            {
                await dbContext.Vehicles
                    .AddAsync(entity, cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
    }

    public sealed class Handler(ICreateVehicle dataAccess)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {


            var entity = VehicleEntity.Create(
                request.TenantId,
                Guid.NewGuid().ToString("N").ToUpperInvariant(),
                request.Name);

            await dataAccess.AddAsync(entity, cancellationToken);
            return Result<Response>.Success(MapResponse(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "vehicle"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateVehicle")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantDriver);
        return endpoints;
    }

    private static Response MapResponse(VehicleEntity entity)
    {
        return new Response(
            entity.TenantId,
            entity.VehicleId,
            entity.Code,
            entity.Name,
            entity.MetadataJson);
    }
}
