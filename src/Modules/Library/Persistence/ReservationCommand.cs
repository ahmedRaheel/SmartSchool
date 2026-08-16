using SmartSchool.Modules.Library.Models;

namespace SmartSchool.Modules.Library.Persistence;

/// <summary>
/// Write-side persistence for Reservation.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ReservationCommand : IReservationCommand
{
    public Task AddAsync(
        Reservation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Reservation create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Reservation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Reservation update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Reservation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Reservation delete persistence has not been connected to the module DbContext.");
    }
}
