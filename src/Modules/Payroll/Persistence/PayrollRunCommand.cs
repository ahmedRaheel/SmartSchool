using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Persistence;

/// <summary>
/// Write-side persistence for PayrollRunEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class PayrollRunCommand : IPayrollRunCommand
{
    public Task AddAsync(
        PayrollRunEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PayrollRunEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        PayrollRunEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PayrollRunEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        PayrollRunEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PayrollRunEntity delete persistence has not been connected to the module DbContext.");
    }
}
