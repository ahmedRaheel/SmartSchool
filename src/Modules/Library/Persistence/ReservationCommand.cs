using SmartSchool.Modules.Library.Models;

namespace SmartSchool.Modules.Library.Persistence;

/// <summary>
/// Write-side persistence for ReservationEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ReservationCommand : IReservationCommand
{
    public Task AddAsync(
        ReservationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ReservationEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ReservationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ReservationEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ReservationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ReservationEntity delete persistence has not been connected to the module DbContext.");
    }
}
