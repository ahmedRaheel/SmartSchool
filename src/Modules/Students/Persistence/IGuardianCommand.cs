using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence;

public interface IGuardianCommand
{
    Task AddAsync(
        Guardian entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Guardian entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Guardian entity,
        CancellationToken cancellationToken);
}
