using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Persistence;

public interface IWorkflowDefinitionCommand
{
    Task AddAsync(
        WorkflowDefinition entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        WorkflowDefinition entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        WorkflowDefinition entity,
        CancellationToken cancellationToken);
}
