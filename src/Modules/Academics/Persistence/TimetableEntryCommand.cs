using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Write-side persistence for TimetableEntry.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class TimetableEntryCommand : ITimetableEntryCommand
{
    public Task AddAsync(
        TimetableEntry entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TimetableEntry create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        TimetableEntry entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TimetableEntry update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        TimetableEntry entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TimetableEntry delete persistence has not been connected to the module DbContext.");
    }
}
