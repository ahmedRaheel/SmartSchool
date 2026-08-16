using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Persistence;

/// <summary>
/// Write-side persistence for Approval.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ApprovalCommand : IApprovalCommand
{
    public Task AddAsync(
        Approval entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Approval create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Approval entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Approval update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Approval entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Approval delete persistence has not been connected to the module DbContext.");
    }
}
