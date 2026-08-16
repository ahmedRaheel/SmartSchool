using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Persistence;

public interface IIncrementCommand
{
    Task AddAsync(
        Increment entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Increment entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Increment entity,
        CancellationToken cancellationToken);
}
