using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Inventory.Models;

namespace SmartSchool.Modules.Inventory.Persistence;

/// <summary>
/// EF-backed write persistence for PurchaseOrderEntity.
/// </summary>
public sealed class PurchaseOrderCommand(IEfMockStore store) : IPurchaseOrderCommand
{
	public Task AddAsync(PurchaseOrderEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(PurchaseOrderEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(PurchaseOrderEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
