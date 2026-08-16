using SmartSchool.Modules.Activities.Models;

namespace SmartSchool.Modules.Activities.Persistence;

/// <summary>
/// Write-side persistence for Activity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ActivityCommand : IActivityCommand
{
    public Task AddAsync(
        Activity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Activity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Activity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Activity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Activity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Activity delete persistence has not been connected to the module DbContext.");
    }
}
