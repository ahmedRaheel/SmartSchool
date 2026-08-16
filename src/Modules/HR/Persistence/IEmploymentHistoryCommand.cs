using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

public interface IEmploymentHistoryCommand
{
    Task AddAsync(
        EmploymentHistory entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        EmploymentHistory entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        EmploymentHistory entity,
        CancellationToken cancellationToken);
}
