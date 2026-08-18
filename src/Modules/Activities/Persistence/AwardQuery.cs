using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Activities.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Activities.Persistence;

/// <summary>
/// EF-backed read persistence for AwardEntity.
/// </summary>
public sealed class AwardQuery(IEfMockStore store) : IAwardQuery
{
	public Task<AwardEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<AwardEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<AwardEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<AwardEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<AwardEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
