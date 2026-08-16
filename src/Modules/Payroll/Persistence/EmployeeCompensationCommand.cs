using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Persistence;

/// <summary>
/// Write-side persistence for EmployeeCompensation.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class EmployeeCompensationCommand : IEmployeeCompensationCommand
{
    public Task AddAsync(
        EmployeeCompensation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "EmployeeCompensation create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        EmployeeCompensation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "EmployeeCompensation update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        EmployeeCompensation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "EmployeeCompensation delete persistence has not been connected to the module DbContext.");
    }
}
