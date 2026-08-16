using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Write-side persistence for LeaveRequest.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class LeaveRequestCommand : ILeaveRequestCommand
{
    public Task AddAsync(
        LeaveRequest entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LeaveRequest create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        LeaveRequest entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LeaveRequest update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        LeaveRequest entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LeaveRequest delete persistence has not been connected to the module DbContext.");
    }
}
