using SmartSchool.Modules.Activities.Models;

namespace SmartSchool.Modules.Activities.Persistence;

/// <summary>
/// Write-side persistence for ActivityEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ActivityCommand : IActivityCommand
{
    public Task AddAsync(
        ActivityEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ActivityEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ActivityEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ActivityEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ActivityEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ActivityEntity delete persistence has not been connected to the module DbContext.");
    }
}
