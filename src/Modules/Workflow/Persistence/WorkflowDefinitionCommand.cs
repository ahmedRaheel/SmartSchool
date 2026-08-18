using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Persistence;

/// <summary>
/// EF-backed write persistence for WorkflowDefinitionEntity.
/// </summary>
public sealed class WorkflowDefinitionCommand(IEfMockStore store) : IWorkflowDefinitionCommand
{
	public Task AddAsync(WorkflowDefinitionEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(WorkflowDefinitionEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(WorkflowDefinitionEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
