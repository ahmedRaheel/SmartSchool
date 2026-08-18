using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Workflow.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Workflow.Persistence;

/// <summary>
/// EF-backed read persistence for WorkflowDefinitionEntity.
/// </summary>
public sealed class WorkflowDefinitionQuery(IEfMockStore store) : IWorkflowDefinitionQuery
{
	public Task<WorkflowDefinitionEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<WorkflowDefinitionEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<WorkflowDefinitionEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<WorkflowDefinitionEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<WorkflowDefinitionEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
