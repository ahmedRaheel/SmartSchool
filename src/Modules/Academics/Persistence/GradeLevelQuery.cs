using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// EF-backed read persistence for GradeLevelEntity.
/// </summary>
public sealed class GradeLevelQuery(IEfMockStore store) : IGradeLevelQuery
{
	public Task<GradeLevelEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<GradeLevelEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<GradeLevelEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<GradeLevelEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<GradeLevelEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
