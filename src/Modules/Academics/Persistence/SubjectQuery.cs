using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// EF-backed read persistence for SubjectEntity.
/// </summary>
public sealed class SubjectQuery(IEfMockStore store) : ISubjectQuery
{
	public Task<SubjectEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<SubjectEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<SubjectEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<SubjectEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<SubjectEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
