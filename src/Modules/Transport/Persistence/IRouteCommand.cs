using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Persistence;

public interface IRouteCommand
{
    Task AddAsync(
        Route entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Route entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Route entity,
        CancellationToken cancellationToken);
}
