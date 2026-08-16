using SmartSchool.Modules.Inventory.Models;

namespace SmartSchool.Modules.Inventory.Persistence;

public interface IItemCommand
{
    Task AddAsync(
        Item entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Item entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Item entity,
        CancellationToken cancellationToken);
}
