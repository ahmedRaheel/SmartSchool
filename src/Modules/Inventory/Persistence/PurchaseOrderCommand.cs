using SmartSchool.Modules.Inventory.Models;

namespace SmartSchool.Modules.Inventory.Persistence;

/// <summary>
/// Write-side persistence for PurchaseOrderEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class PurchaseOrderCommand : IPurchaseOrderCommand
{
    public Task AddAsync(
        PurchaseOrderEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PurchaseOrderEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        PurchaseOrderEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PurchaseOrderEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        PurchaseOrderEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PurchaseOrderEntity delete persistence has not been connected to the module DbContext.");
    }
}
