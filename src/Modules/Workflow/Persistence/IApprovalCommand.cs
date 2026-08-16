using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Persistence;

public interface IApprovalCommand
{
    Task AddAsync(
        Approval entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Approval entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Approval entity,
        CancellationToken cancellationToken);
}
