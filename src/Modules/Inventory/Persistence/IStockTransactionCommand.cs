using SmartSchool.Modules.Inventory.Models;

namespace SmartSchool.Modules.Inventory.Persistence;

public interface IStockTransactionCommand
{
    Task AddAsync(
        StockTransaction entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        StockTransaction entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        StockTransaction entity,
        CancellationToken cancellationToken);
}
