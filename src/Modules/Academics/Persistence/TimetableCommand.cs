using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Write-side persistence for TimetableEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class TimetableCommand : ITimetableCommand
{
    public Task AddAsync(
        TimetableEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TimetableEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        TimetableEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TimetableEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        TimetableEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TimetableEntity delete persistence has not been connected to the module DbContext.");
    }
}
