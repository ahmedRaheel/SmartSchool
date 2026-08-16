using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Persistence;

public interface IDepartmentCommand
{
    Task AddAsync(
        Department entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Department entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Department entity,
        CancellationToken cancellationToken);
}
