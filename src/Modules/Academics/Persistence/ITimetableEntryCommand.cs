using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

public interface ITimetableEntryCommand
{
    Task AddAsync(
        TimetableEntry entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        TimetableEntry entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        TimetableEntry entity,
        CancellationToken cancellationToken);
}
