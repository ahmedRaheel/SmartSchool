using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Inventory.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Inventory.Features.StockTransaction;

public static class GetStockTransactionById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<StockTransaction> repository)
    {
        public async Task<Result<StockTransaction>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<StockTransaction>.Failure(
                    Error.NotFound("StockTransaction was not found."));
            }

            return Result<StockTransaction>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/inventory/stock-transaction/{id:guid}",
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
            .WithName("GetStockTransactionById")
            .WithTags("Inventory")
            .RequireAuthorization();

        return endpoints;
    }
}
