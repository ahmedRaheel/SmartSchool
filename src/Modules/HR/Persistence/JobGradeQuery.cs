using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// EF-backed read persistence for JobGradeEntity.
/// </summary>
public sealed class JobGradeQuery(IEfMockStore store) : IJobGradeQuery
{
	public Task<JobGradeEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<JobGradeEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<JobGradeEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<JobGradeEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<JobGradeEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
