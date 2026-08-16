using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

public interface ITimetableCommand
{
    Task AddAsync(
        Timetable entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Timetable entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Timetable entity,
        CancellationToken cancellationToken);
}
