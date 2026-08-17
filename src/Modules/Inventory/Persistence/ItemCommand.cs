using SmartSchool.Modules.Inventory.Models;

namespace SmartSchool.Modules.Inventory.Persistence;

/// <summary>
/// Write-side persistence for ItemEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ItemCommand : IItemCommand
{
    public Task AddAsync(
        ItemEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ItemEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ItemEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ItemEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ItemEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ItemEntity delete persistence has not been connected to the module DbContext.");
    }
}
