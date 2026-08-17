using SmartSchool.Modules.Inventory.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Inventory.Persistence;

/// <summary>
/// Read-side persistence for ItemEntity.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Inventory module.
/// </summary>
public sealed class ItemQuery : IItemQuery
{
    public Task<ItemEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ItemEntity read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<ItemEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ItemEntity paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ItemEntity uniqueness persistence has not been connected to the module DbContext.");
    }
}
