using SmartSchool.Modules.Inventory.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Inventory.Persistence;

/// <summary>
/// Read-side persistence for Item.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Inventory module.
/// </summary>
public sealed class ItemQuery : IItemQuery
{
    public Task<Item?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Item read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Item>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Item paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Item uniqueness persistence has not been connected to the module DbContext.");
    }
}
