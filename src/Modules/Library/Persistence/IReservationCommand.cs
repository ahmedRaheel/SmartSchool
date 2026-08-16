using SmartSchool.Modules.Library.Models;

namespace SmartSchool.Modules.Library.Persistence;

public interface IReservationCommand
{
    Task AddAsync(
        Reservation entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Reservation entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Reservation entity,
        CancellationToken cancellationToken);
}
