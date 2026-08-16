using SmartSchool.Modules.Inventory.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Inventory.Persistence;

/// <summary>
/// Read-side persistence for StockTransaction.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Inventory module.
/// </summary>
public sealed class StockTransactionQuery : IStockTransactionQuery
{
    public Task<StockTransaction?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StockTransaction read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<StockTransaction>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StockTransaction paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StockTransaction uniqueness persistence has not been connected to the module DbContext.");
    }
}
