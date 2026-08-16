using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Persistence;

public interface IAssignmentCommand
{
    Task AddAsync(
        Assignment entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Assignment entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Assignment entity,
        CancellationToken cancellationToken);
}
