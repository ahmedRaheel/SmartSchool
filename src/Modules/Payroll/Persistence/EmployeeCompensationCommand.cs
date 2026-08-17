using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Persistence;

/// <summary>
/// Write-side persistence for EmployeeCompensationEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class EmployeeCompensationCommand : IEmployeeCompensationCommand
{
    public Task AddAsync(
        EmployeeCompensationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "EmployeeCompensationEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        EmployeeCompensationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "EmployeeCompensationEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        EmployeeCompensationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "EmployeeCompensationEntity delete persistence has not been connected to the module DbContext.");
    }
}
