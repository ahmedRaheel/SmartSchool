using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Inventory.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Inventory.Persistence;

/// <summary>
/// EF-backed read persistence for StockTransactionEntity.
/// </summary>
public sealed class StockTransactionQuery(IEfMockStore store) : IStockTransactionQuery
{
	public Task<StockTransactionEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<StockTransactionEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<StockTransactionEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<StockTransactionEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<StockTransactionEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
