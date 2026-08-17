using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Write-side persistence for TimetableEntryEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class TimetableEntryCommand : ITimetableEntryCommand
{
    public Task AddAsync(
        TimetableEntryEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TimetableEntryEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        TimetableEntryEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TimetableEntryEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        TimetableEntryEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TimetableEntryEntity delete persistence has not been connected to the module DbContext.");
    }
}
