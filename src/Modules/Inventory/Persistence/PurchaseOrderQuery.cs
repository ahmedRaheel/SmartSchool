using SmartSchool.Modules.Inventory.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Inventory.Persistence;

/// <summary>
/// Read-side persistence for PurchaseOrderEntity.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Inventory module.
/// </summary>
public sealed class PurchaseOrderQuery : IPurchaseOrderQuery
{
    public Task<PurchaseOrderEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PurchaseOrderEntity read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<PurchaseOrderEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PurchaseOrderEntity paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PurchaseOrderEntity uniqueness persistence has not been connected to the module DbContext.");
    }
}
