using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Activities.Contracts;
using SmartSchool.Modules.Activities.Models;
using SmartSchool.Modules.Activities.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Activities.Features.Activity;

public static class GetActivityById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<ActivityResponse>>;

    public sealed class Handler(IActivityQuery entityQuery)
        : IRequestHandler<Query, Result<ActivityResponse>>
    {
        public async Task<Result<ActivityResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<ActivityResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Activity))));
            }
            return Result<ActivityResponse>.Success(ActivityResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "activity"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<ActivityResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetActivityById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
