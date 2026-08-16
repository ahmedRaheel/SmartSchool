using SmartSchool.Modules.Inventory.Models;

namespace SmartSchool.Modules.Inventory.Persistence;

/// <summary>
/// Write-side persistence for PurchaseOrder.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class PurchaseOrderCommand : IPurchaseOrderCommand
{
    public Task AddAsync(
        PurchaseOrder entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PurchaseOrder create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        PurchaseOrder entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PurchaseOrder update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        PurchaseOrder entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PurchaseOrder delete persistence has not been connected to the module DbContext.");
    }
}
