using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Persistence;

/// <summary>
/// Write-side persistence for ApprovalEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ApprovalCommand : IApprovalCommand
{
    public Task AddAsync(
        ApprovalEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ApprovalEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ApprovalEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ApprovalEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ApprovalEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ApprovalEntity delete persistence has not been connected to the module DbContext.");
    }
}
