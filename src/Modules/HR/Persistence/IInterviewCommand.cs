using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

public interface IInterviewCommand
{
    Task AddAsync(
        Interview entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Interview entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Interview entity,
        CancellationToken cancellationToken);
}
