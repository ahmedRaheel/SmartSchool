using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Learning.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Learning.Persistence;

/// <summary>
/// EF-backed read persistence for AssignmentSubmissionEntity.
/// </summary>
public sealed class AssignmentSubmissionQuery(IEfMockStore store) : IAssignmentSubmissionQuery
{
	public Task<AssignmentSubmissionEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<AssignmentSubmissionEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<AssignmentSubmissionEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<AssignmentSubmissionEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<AssignmentSubmissionEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
