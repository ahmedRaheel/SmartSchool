using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// EF-backed read persistence for CandidateEntity.
/// </summary>
public sealed class CandidateQuery(IEfMockStore store) : ICandidateQuery
{
	public Task<CandidateEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<CandidateEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<CandidateEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<CandidateEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<CandidateEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
