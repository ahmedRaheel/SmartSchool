using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

public interface ILeaveRequestCommand
{
    Task AddAsync(
        LeaveRequest entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        LeaveRequest entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        LeaveRequest entity,
        CancellationToken cancellationToken);
}
