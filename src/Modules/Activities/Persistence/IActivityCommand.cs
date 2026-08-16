using SmartSchool.Modules.Activities.Models;

namespace SmartSchool.Modules.Activities.Persistence;

public interface IActivityCommand
{
    Task AddAsync(
        Activity entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Activity entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Activity entity,
        CancellationToken cancellationToken);
}
