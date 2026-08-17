using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Write-side persistence for JobEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class JobCommand : IJobCommand
{
    public Task AddAsync(
        JobEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "JobEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        JobEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "JobEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        JobEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "JobEntity delete persistence has not been connected to the module DbContext.");
    }
}
