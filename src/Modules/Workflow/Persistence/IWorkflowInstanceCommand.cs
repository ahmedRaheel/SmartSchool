using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Persistence;

public interface IWorkflowInstanceCommand
{
    Task AddAsync(
        WorkflowInstance entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        WorkflowInstance entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        WorkflowInstance entity,
        CancellationToken cancellationToken);
}
