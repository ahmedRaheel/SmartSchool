using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Persistence;

/// <summary>
/// Write-side persistence for Stop.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class StopCommand : IStopCommand
{
    public Task AddAsync(
        Stop entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Stop create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Stop entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Stop update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Stop entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Stop delete persistence has not been connected to the module DbContext.");
    }
}
