using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

public interface IJobCommand
{
    Task AddAsync(
        Job entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Job entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Job entity,
        CancellationToken cancellationToken);
}
