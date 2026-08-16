using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Persistence;

/// <summary>
/// Write-side persistence for PayrollRun.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class PayrollRunCommand : IPayrollRunCommand
{
    public Task AddAsync(
        PayrollRun entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PayrollRun create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        PayrollRun entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PayrollRun update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        PayrollRun entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PayrollRun delete persistence has not been connected to the module DbContext.");
    }
}
