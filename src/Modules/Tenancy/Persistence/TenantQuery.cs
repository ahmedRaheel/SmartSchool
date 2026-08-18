using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Tenancy.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Tenancy.Persistence;

/// <summary>
/// EF-backed read persistence for TenantEntity.
/// </summary>
public sealed class TenantQuery(IEfMockStore store) : ITenantQuery
{
	public Task<TenantEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<TenantEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<TenantEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<TenantEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<TenantEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
