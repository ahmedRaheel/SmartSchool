using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Persistence;

/// <summary>
/// Write-side persistence for WorkflowDefinitionEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class WorkflowDefinitionCommand : IWorkflowDefinitionCommand
{
    public Task AddAsync(
        WorkflowDefinitionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "WorkflowDefinitionEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        WorkflowDefinitionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "WorkflowDefinitionEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        WorkflowDefinitionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "WorkflowDefinitionEntity delete persistence has not been connected to the module DbContext.");
    }
}
