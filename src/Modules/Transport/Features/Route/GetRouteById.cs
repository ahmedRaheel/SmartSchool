using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Transport.Contracts;
using SmartSchool.Modules.Transport.Models;
using SmartSchool.Modules.Transport.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Transport.Features.Route;

public static class GetRouteById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<RouteResponse>>;

    public sealed class Handler(IRouteQuery entityQuery)
        : IRequestHandler<Query, Result<RouteResponse>>
    {
        public async Task<Result<RouteResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<RouteResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Route))));
            }
            return Result<RouteResponse>.Success(RouteResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "route"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<RouteResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetRouteById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
