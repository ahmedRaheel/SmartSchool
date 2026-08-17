using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Persistence;

/// <summary>
/// Write-side persistence for WorkflowInstanceEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class WorkflowInstanceCommand : IWorkflowInstanceCommand
{
    public Task AddAsync(
        WorkflowInstanceEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "WorkflowInstanceEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        WorkflowInstanceEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "WorkflowInstanceEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        WorkflowInstanceEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "WorkflowInstanceEntity delete persistence has not been connected to the module DbContext.");
    }
}
