using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Persistence;

/// <summary>
/// Write-side persistence for Payslip.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class PayslipCommand : IPayslipCommand
{
    public Task AddAsync(
        Payslip entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Payslip create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Payslip entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Payslip update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Payslip entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Payslip delete persistence has not been connected to the module DbContext.");
    }
}
