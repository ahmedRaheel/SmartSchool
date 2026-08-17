using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Inventory.Contracts;
using SmartSchool.Modules.Inventory.Models;
using SmartSchool.Modules.Inventory.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Inventory.Features.Item;

public static class GetItemById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<ItemResponse>>;

    public sealed class Handler(IItemQuery entityQuery)
        : IRequestHandler<Query, Result<ItemResponse>>
    {
        public async Task<Result<ItemResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<ItemResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Item))));
            }
            return Result<ItemResponse>.Success(ItemResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "item"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<ItemResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetItemById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
