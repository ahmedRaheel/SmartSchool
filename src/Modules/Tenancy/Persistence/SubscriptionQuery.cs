using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Tenancy.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Tenancy.Persistence;

/// <summary>
/// EF-backed read persistence for SubscriptionEntity.
/// </summary>
public sealed class SubscriptionQuery(IEfMockStore store) : ISubscriptionQuery
{
	public Task<SubscriptionEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<SubscriptionEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<SubscriptionEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<SubscriptionEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<SubscriptionEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
