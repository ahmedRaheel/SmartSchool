using SmartSchool.Modules.Inventory.Models;

namespace SmartSchool.Modules.Inventory.Persistence;

/// <summary>
/// Write-side persistence for StockTransactionEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class StockTransactionCommand : IStockTransactionCommand
{
    public Task AddAsync(
        StockTransactionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StockTransactionEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        StockTransactionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StockTransactionEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        StockTransactionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StockTransactionEntity delete persistence has not been connected to the module DbContext.");
    }
}
