using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

public interface IScholarshipCommand
{
    Task AddAsync(
        Scholarship entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Scholarship entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Scholarship entity,
        CancellationToken cancellationToken);
}
