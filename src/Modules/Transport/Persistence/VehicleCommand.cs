using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Persistence;

/// <summary>
/// Write-side persistence for Vehicle.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class VehicleCommand : IVehicleCommand
{
    public Task AddAsync(
        Vehicle entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Vehicle create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Vehicle entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Vehicle update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Vehicle entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Vehicle delete persistence has not been connected to the module DbContext.");
    }
}
