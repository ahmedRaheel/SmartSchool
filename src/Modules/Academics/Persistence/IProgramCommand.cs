using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

public interface IProgramCommand
{
    Task AddAsync(
        Program entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Program entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Program entity,
        CancellationToken cancellationToken);
}
