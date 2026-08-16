using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

public interface ITermCommand
{
    Task AddAsync(
        Term entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Term entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Term entity,
        CancellationToken cancellationToken);
}
