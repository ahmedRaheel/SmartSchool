using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Tenancy.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Tenancy.Persistence;

/// <summary>
/// EF-backed read persistence for CampusBrandingEntity.
/// </summary>
public sealed class CampusBrandingQuery(IEfMockStore store) : ICampusBrandingQuery
{
	public Task<CampusBrandingEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<CampusBrandingEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<CampusBrandingEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<CampusBrandingEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<CampusBrandingEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
