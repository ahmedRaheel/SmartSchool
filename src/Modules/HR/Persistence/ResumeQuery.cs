using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// EF-backed read persistence for ResumeEntity.
/// </summary>
public sealed class ResumeQuery(IEfMockStore store) : IResumeQuery
{
	public Task<ResumeEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<ResumeEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<ResumeEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<ResumeEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<ResumeEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
