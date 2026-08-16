using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Persistence;

public interface IStopCommand
{
    Task AddAsync(
        Stop entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Stop entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Stop entity,
        CancellationToken cancellationToken);
}
