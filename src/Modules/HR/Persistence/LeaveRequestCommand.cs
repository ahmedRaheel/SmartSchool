using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Write-side persistence for LeaveRequestEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class LeaveRequestCommand : ILeaveRequestCommand
{
    public Task AddAsync(
        LeaveRequestEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LeaveRequestEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        LeaveRequestEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LeaveRequestEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        LeaveRequestEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LeaveRequestEntity delete persistence has not been connected to the module DbContext.");
    }
}
