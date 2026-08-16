using SmartSchool.Modules.Inventory.Models;

namespace SmartSchool.Modules.Inventory.Persistence;

public interface IPurchaseOrderCommand
{
    Task AddAsync(
        PurchaseOrder entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        PurchaseOrder entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        PurchaseOrder entity,
        CancellationToken cancellationToken);
}
