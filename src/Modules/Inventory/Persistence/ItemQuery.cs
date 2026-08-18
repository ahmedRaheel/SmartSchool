using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Inventory.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Inventory.Persistence;

/// <summary>
/// EF-backed read persistence for ItemEntity.
/// </summary>
public sealed class ItemQuery(IEfMockStore store) : IItemQuery
{
	public Task<ItemEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<ItemEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<ItemEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<ItemEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<ItemEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
