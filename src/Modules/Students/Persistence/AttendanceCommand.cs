using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence;

/// <summary>
/// Write-side persistence for Attendance.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class AttendanceCommand : IAttendanceCommand
{
    public Task AddAsync(
        Attendance entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Attendance create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Attendance entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Attendance update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Attendance entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Attendance delete persistence has not been connected to the module DbContext.");
    }
}
