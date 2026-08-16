using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Write-side persistence for Employee.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class EmployeeCommand : IEmployeeCommand
{
    public Task AddAsync(
        Employee entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Employee create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Employee entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Employee update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Employee entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Employee delete persistence has not been connected to the module DbContext.");
    }
}
