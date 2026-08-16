using SmartSchool.Modules.Admissions.Models;

namespace SmartSchool.Modules.Admissions.Persistence;

public interface IApplicationCommand
{
    Task AddAsync(
        Application entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Application entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Application entity,
        CancellationToken cancellationToken);
}
