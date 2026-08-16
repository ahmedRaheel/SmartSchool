using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Persistence;

public interface IPayrollRunCommand
{
    Task AddAsync(
        PayrollRun entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        PayrollRun entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        PayrollRun entity,
        CancellationToken cancellationToken);
}
