using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Persistence;

public interface IWorkflowStepCommand
{
    Task AddAsync(
        WorkflowStep entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        WorkflowStep entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        WorkflowStep entity,
        CancellationToken cancellationToken);
}
