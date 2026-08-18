using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Inventory.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Inventory.Persistence;

/// <summary>
/// EF-backed read persistence for PurchaseOrderEntity.
/// </summary>
public sealed class PurchaseOrderQuery(IEfMockStore store) : IPurchaseOrderQuery
{
	public Task<PurchaseOrderEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<PurchaseOrderEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<PurchaseOrderEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<PurchaseOrderEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<PurchaseOrderEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
