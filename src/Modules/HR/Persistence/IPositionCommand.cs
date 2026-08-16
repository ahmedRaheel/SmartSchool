using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

public interface IPositionCommand
{
    Task AddAsync(
        Position entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Position entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Position entity,
        CancellationToken cancellationToken);
}
