using SmartSchool.Modules.Inventory.Models;

namespace SmartSchool.Modules.Inventory.Persistence;

/// <summary>
/// Write-side persistence for Item.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ItemCommand : IItemCommand
{
    public Task AddAsync(
        Item entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Item create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Item entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Item update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Item entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Item delete persistence has not been connected to the module DbContext.");
    }
}
