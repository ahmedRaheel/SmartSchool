using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Inventory.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Inventory.Features.PurchaseOrder;

public static class GetPurchaseOrderById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<PurchaseOrder> repository)
    {
        public async Task<Result<PurchaseOrder>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<PurchaseOrder>.Failure(
                    Error.NotFound("PurchaseOrder was not found."));
            }

            return Result<PurchaseOrder>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/inventory/purchase-order/{id:guid}",
                async (
                    Guid id,
                    Guid tenantId,
                    Handler handler,
                    CancellationToken cancellationToken) =>
                {
                    var query = new Query(tenantId, id);

                    var result = await handler.HandleAsync(
                        query,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("GetPurchaseOrderById")
            .WithTags("Inventory")
            .RequireAuthorization();

        return endpoints;
    }
}
