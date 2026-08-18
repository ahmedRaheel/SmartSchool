using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Workflow.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Workflow.Persistence;

/// <summary>
/// EF-backed read persistence for ApprovalEntity.
/// </summary>
public sealed class ApprovalQuery(IEfMockStore store) : IApprovalQuery
{
	public Task<ApprovalEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<ApprovalEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<ApprovalEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<ApprovalEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<ApprovalEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
