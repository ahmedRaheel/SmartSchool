using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Persistence;

/// <summary>
/// EF-backed write persistence for WorkflowInstanceEntity.
/// </summary>
public sealed class WorkflowInstanceCommand(IEfMockStore store) : IWorkflowInstanceCommand
{
	public Task AddAsync(WorkflowInstanceEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(WorkflowInstanceEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(WorkflowInstanceEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
