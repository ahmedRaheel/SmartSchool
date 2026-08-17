using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Transport.Contracts;
using SmartSchool.Modules.Transport.Models;
using SmartSchool.Modules.Transport.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Transport.Features.Stop;

public static class GetStopById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<StopResponse>>;

    public sealed class Handler(IStopQuery entityQuery)
        : IRequestHandler<Query, Result<StopResponse>>
    {
        public async Task<Result<StopResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<StopResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Stop))));
            }
            return Result<StopResponse>.Success(StopResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "stop"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<StopResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetStopById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
