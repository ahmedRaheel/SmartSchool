using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Transport.Contracts;
using SmartSchool.Modules.Transport.Models;
using SmartSchool.Modules.Transport.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Transport.Features.Vehicle;

public static class GetVehicleById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<VehicleResponse>>;

    public sealed class Handler(IVehicleQuery entityQuery)
        : IRequestHandler<Query, Result<VehicleResponse>>
    {
        public async Task<Result<VehicleResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<VehicleResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Vehicle))));
            }
            return Result<VehicleResponse>.Success(VehicleResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "vehicle"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<VehicleResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetVehicleById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
