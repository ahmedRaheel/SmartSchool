using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Persistence;

/// <summary>
/// Write-side persistence for WorkflowInstance.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class WorkflowInstanceCommand : IWorkflowInstanceCommand
{
    public Task AddAsync(
        WorkflowInstance entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "WorkflowInstance create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        WorkflowInstance entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "WorkflowInstance update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        WorkflowInstance entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "WorkflowInstance delete persistence has not been connected to the module DbContext.");
    }
}
