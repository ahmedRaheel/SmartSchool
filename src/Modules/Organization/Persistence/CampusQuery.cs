using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Persistence;

/// <summary>
/// EF-backed read persistence for CampusEntity.
/// </summary>
public sealed class CampusQuery(IEfMockStore store) : ICampusQuery
{
	public Task<CampusEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<CampusEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<CampusEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<CampusEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<CampusEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
