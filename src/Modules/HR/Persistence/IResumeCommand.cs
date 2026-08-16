using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

public interface IResumeCommand
{
    Task AddAsync(
        Resume entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Resume entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Resume entity,
        CancellationToken cancellationToken);
}
