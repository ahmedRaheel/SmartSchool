using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Persistence;

/// <summary>
/// Write-side persistence for RouteEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class RouteCommand : IRouteCommand
{
    public Task AddAsync(
        RouteEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "RouteEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        RouteEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "RouteEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        RouteEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "RouteEntity delete persistence has not been connected to the module DbContext.");
    }
}
