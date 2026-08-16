using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence;

public interface IAttendanceCommand
{
    Task AddAsync(
        Attendance entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Attendance entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Attendance entity,
        CancellationToken cancellationToken);
}
