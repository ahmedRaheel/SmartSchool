using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Write-side persistence for EmploymentHistory.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class EmploymentHistoryCommand : IEmploymentHistoryCommand
{
    public Task AddAsync(
        EmploymentHistory entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "EmploymentHistory create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        EmploymentHistory entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "EmploymentHistory update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        EmploymentHistory entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "EmploymentHistory delete persistence has not been connected to the module DbContext.");
    }
}
