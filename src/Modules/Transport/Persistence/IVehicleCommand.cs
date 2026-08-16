using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Persistence;

public interface IVehicleCommand
{
    Task AddAsync(
        Vehicle entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Vehicle entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Vehicle entity,
        CancellationToken cancellationToken);
}
