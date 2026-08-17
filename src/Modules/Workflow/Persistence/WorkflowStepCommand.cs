using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Persistence;

/// <summary>
/// Write-side persistence for WorkflowStepEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class WorkflowStepCommand : IWorkflowStepCommand
{
    public Task AddAsync(
        WorkflowStepEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "WorkflowStepEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        WorkflowStepEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "WorkflowStepEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        WorkflowStepEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "WorkflowStepEntity delete persistence has not been connected to the module DbContext.");
    }
}
