using SmartSchool.Modules.Inventory.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Inventory.Persistence;

public interface IItemQuery
{
    Task<Item?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResult<Item>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken);
}
