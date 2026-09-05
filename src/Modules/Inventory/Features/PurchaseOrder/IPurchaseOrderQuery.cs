using System.Threading.Tasks;
using SmartSchool.Modules.Inventory.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Inventory.Features.PurchaseOrder;

/// <summary>
/// Defines query persistence operations for PurchaseOrderEntity.
/// </summary>
public interface IPurchaseOrderQuery
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<PurchaseOrderEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<PagedResult<PurchaseOrderEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken);
}
