using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

public interface IEmployeeCommand
{
    Task AddAsync(
        Employee entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Employee entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Employee entity,
        CancellationToken cancellationToken);
}
