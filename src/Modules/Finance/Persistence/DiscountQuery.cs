using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Finance.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// EF-backed read persistence for DiscountEntity.
/// </summary>
public sealed class DiscountQuery(IEfMockStore store) : IDiscountQuery
{
	public Task<DiscountEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<DiscountEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<DiscountEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<DiscountEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<DiscountEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
