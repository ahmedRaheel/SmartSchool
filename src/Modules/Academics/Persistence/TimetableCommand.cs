using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Write-side persistence for Timetable.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class TimetableCommand : ITimetableCommand
{
    public Task AddAsync(
        Timetable entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Timetable create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Timetable entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Timetable update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Timetable entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Timetable delete persistence has not been connected to the module DbContext.");
    }
}
