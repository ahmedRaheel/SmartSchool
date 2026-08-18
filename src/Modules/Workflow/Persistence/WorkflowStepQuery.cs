using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Workflow.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Workflow.Persistence;

/// <summary>
/// EF-backed read persistence for WorkflowStepEntity.
/// </summary>
public sealed class WorkflowStepQuery(IEfMockStore store) : IWorkflowStepQuery
{
	public Task<WorkflowStepEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<WorkflowStepEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<WorkflowStepEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<WorkflowStepEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<WorkflowStepEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
