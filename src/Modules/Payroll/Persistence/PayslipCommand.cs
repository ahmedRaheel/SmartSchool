using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Persistence;

/// <summary>
/// Write-side persistence for PayslipEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class PayslipCommand : IPayslipCommand
{
    public Task AddAsync(
        PayslipEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PayslipEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        PayslipEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PayslipEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        PayslipEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PayslipEntity delete persistence has not been connected to the module DbContext.");
    }
}
