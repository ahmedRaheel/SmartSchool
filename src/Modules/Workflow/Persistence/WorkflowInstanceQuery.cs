using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Workflow.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Workflow.Persistence;

/// <summary>
/// EF-backed read persistence for WorkflowInstanceEntity.
/// </summary>
public sealed class WorkflowInstanceQuery(IEfMockStore store) : IWorkflowInstanceQuery
{
	public Task<WorkflowInstanceEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<WorkflowInstanceEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<WorkflowInstanceEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<WorkflowInstanceEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<WorkflowInstanceEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
