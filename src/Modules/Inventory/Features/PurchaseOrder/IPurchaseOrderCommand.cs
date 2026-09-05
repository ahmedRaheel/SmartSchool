using System.Threading.Tasks;
using SmartSchool.Modules.Inventory.Models;

namespace SmartSchool.Modules.Inventory.Features.PurchaseOrder;

/// <summary>
/// Defines command persistence operations for PurchaseOrderEntity.
/// </summary>
public interface IPurchaseOrderCommand
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task AddAsync(
        PurchaseOrderEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task UpdateAsync(
        PurchaseOrderEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task DeleteAsync(
        PurchaseOrderEntity entity,
        CancellationToken cancellationToken);
}
