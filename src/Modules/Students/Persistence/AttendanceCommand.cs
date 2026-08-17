using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence;

/// <summary>
/// Write-side persistence for AttendanceEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class AttendanceCommand : IAttendanceCommand
{
    public Task AddAsync(
        AttendanceEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AttendanceEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        AttendanceEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AttendanceEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        AttendanceEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AttendanceEntity delete persistence has not been connected to the module DbContext.");
    }
}
