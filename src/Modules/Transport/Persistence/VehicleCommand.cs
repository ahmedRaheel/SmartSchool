using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Persistence;

/// <summary>
/// Write-side persistence for VehicleEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class VehicleCommand : IVehicleCommand
{
    public Task AddAsync(
        VehicleEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "VehicleEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        VehicleEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "VehicleEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        VehicleEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "VehicleEntity delete persistence has not been connected to the module DbContext.");
    }
}
