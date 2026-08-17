using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Write-side persistence for EmployeeEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class EmployeeCommand : IEmployeeCommand
{
    public Task AddAsync(
        EmployeeEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "EmployeeEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        EmployeeEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "EmployeeEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        EmployeeEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "EmployeeEntity delete persistence has not been connected to the module DbContext.");
    }
}
