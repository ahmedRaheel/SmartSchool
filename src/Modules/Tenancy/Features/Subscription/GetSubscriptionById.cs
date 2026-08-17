using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Tenancy.Contracts;
using SmartSchool.Modules.Tenancy.Models;
using SmartSchool.Modules.Tenancy.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Tenancy.Features.Subscription;

public static class GetSubscriptionById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<SubscriptionResponse>>;

    public sealed class Handler(ISubscriptionQuery entityQuery)
        : IRequestHandler<Query, Result<SubscriptionResponse>>
    {
        public async Task<Result<SubscriptionResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<SubscriptionResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Subscription))));
            }
            return Result<SubscriptionResponse>.Success(SubscriptionResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "subscription"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<SubscriptionResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetSubscriptionById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
