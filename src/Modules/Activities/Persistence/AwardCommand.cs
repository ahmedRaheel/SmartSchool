using SmartSchool.Modules.Activities.Models;

namespace SmartSchool.Modules.Activities.Persistence;

/// <summary>
/// Write-side persistence for AwardEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class AwardCommand : IAwardCommand
{
    public Task AddAsync(
        AwardEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AwardEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        AwardEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AwardEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        AwardEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AwardEntity delete persistence has not been connected to the module DbContext.");
    }
}
