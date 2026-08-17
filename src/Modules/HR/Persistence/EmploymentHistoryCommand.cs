using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Write-side persistence for EmploymentHistoryEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class EmploymentHistoryCommand : IEmploymentHistoryCommand
{
    public Task AddAsync(
        EmploymentHistoryEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "EmploymentHistoryEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        EmploymentHistoryEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "EmploymentHistoryEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        EmploymentHistoryEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "EmploymentHistoryEntity delete persistence has not been connected to the module DbContext.");
    }
}
