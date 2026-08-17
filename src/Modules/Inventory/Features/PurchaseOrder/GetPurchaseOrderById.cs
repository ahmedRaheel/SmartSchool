using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Inventory.Contracts;
using SmartSchool.Modules.Inventory.Models;
using SmartSchool.Modules.Inventory.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Inventory.Features.PurchaseOrder;

public static class GetPurchaseOrderById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<PurchaseOrderResponse>>;

    public sealed class Handler(IPurchaseOrderQuery entityQuery)
        : IRequestHandler<Query, Result<PurchaseOrderResponse>>
    {
        public async Task<Result<PurchaseOrderResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<PurchaseOrderResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(PurchaseOrder))));
            }
            return Result<PurchaseOrderResponse>.Success(PurchaseOrderResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "purchase-order"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<PurchaseOrderResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetPurchaseOrderById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
