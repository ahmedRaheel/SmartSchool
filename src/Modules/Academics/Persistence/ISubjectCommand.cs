using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

public interface ISubjectCommand
{
    Task AddAsync(
        Subject entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Subject entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Subject entity,
        CancellationToken cancellationToken);
}
