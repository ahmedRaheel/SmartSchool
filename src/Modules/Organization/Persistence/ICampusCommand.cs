using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Persistence;

public interface ICampusCommand
{
    Task AddAsync(
        Campus entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Campus entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Campus entity,
        CancellationToken cancellationToken);
}
