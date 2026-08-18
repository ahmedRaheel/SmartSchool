using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// EF-backed read persistence for JobEntity.
/// </summary>
public sealed class JobQuery(IEfMockStore store) : IJobQuery
{
	public Task<JobEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<JobEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<JobEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<JobEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<JobEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
