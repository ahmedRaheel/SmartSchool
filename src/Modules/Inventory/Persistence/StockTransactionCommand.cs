using SmartSchool.Modules.Inventory.Models;

namespace SmartSchool.Modules.Inventory.Persistence;

/// <summary>
/// Write-side persistence for StockTransaction.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class StockTransactionCommand : IStockTransactionCommand
{
    public Task AddAsync(
        StockTransaction entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StockTransaction create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        StockTransaction entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StockTransaction update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        StockTransaction entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StockTransaction delete persistence has not been connected to the module DbContext.");
    }
}
