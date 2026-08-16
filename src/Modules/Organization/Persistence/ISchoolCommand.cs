using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Persistence;

public interface ISchoolCommand
{
    Task AddAsync(
        School entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        School entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        School entity,
        CancellationToken cancellationToken);
}
