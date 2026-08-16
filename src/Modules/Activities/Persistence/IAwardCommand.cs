using SmartSchool.Modules.Activities.Models;

namespace SmartSchool.Modules.Activities.Persistence;

public interface IAwardCommand
{
    Task AddAsync(
        Award entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Award entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Award entity,
        CancellationToken cancellationToken);
}
