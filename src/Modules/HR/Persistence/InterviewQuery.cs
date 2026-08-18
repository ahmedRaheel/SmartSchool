using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// EF-backed read persistence for InterviewEntity.
/// </summary>
public sealed class InterviewQuery(IEfMockStore store) : IInterviewQuery
{
	public Task<InterviewEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<InterviewEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<InterviewEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<InterviewEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<InterviewEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
