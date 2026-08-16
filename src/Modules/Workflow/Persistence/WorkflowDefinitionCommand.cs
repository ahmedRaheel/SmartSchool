using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Persistence;

/// <summary>
/// Write-side persistence for WorkflowDefinition.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class WorkflowDefinitionCommand : IWorkflowDefinitionCommand
{
    public Task AddAsync(
        WorkflowDefinition entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "WorkflowDefinition create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        WorkflowDefinition entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "WorkflowDefinition update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        WorkflowDefinition entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "WorkflowDefinition delete persistence has not been connected to the module DbContext.");
    }
}
