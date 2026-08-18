using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Activities.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Activities.Persistence;

/// <summary>
/// EF-backed read persistence for ActivityEntity.
/// </summary>
public sealed class ActivityQuery(IEfMockStore store) : IActivityQuery
{
	public Task<ActivityEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<ActivityEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<ActivityEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<ActivityEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<ActivityEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
