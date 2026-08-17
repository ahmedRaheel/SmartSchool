using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Persistence;

/// <summary>
/// Write-side persistence for StopEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class StopCommand : IStopCommand
{
    public Task AddAsync(
        StopEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StopEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        StopEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StopEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        StopEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StopEntity delete persistence has not been connected to the module DbContext.");
    }
}
