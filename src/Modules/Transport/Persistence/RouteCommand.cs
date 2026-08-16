using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Persistence;

/// <summary>
/// Write-side persistence for Route.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class RouteCommand : IRouteCommand
{
    public Task AddAsync(
        Route entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Route create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Route entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Route update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Route entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Route delete persistence has not been connected to the module DbContext.");
    }
}
