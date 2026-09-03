using System.Threading.Tasks;
using SmartSchool.Modules.Inventory.Models;

namespace SmartSchool.Modules.Inventory.Features.StockTransaction;

/// <summary>
/// Defines command persistence operations for StockTransactionEntity.
/// </summary>
public interface IStockTransactionCommand
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task AddAsync(
        StockTransactionEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task UpdateAsync(
        StockTransactionEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task DeleteAsync(
        StockTransactionEntity entity,
        CancellationToken cancellationToken);
}
