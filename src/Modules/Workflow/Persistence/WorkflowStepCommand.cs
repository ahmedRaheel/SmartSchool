using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Persistence;

/// <summary>
/// Write-side persistence for WorkflowStep.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class WorkflowStepCommand : IWorkflowStepCommand
{
    public Task AddAsync(
        WorkflowStep entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "WorkflowStep create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        WorkflowStep entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "WorkflowStep update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        WorkflowStep entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "WorkflowStep delete persistence has not been connected to the module DbContext.");
    }
}
