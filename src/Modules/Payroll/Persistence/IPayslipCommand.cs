using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Persistence;

public interface IPayslipCommand
{
    Task AddAsync(
        Payslip entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Payslip entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Payslip entity,
        CancellationToken cancellationToken);
}
