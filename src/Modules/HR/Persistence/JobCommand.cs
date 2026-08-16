using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Write-side persistence for Job.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class JobCommand : IJobCommand
{
    public Task AddAsync(
        Job entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Job create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Job entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Job update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Job entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Job delete persistence has not been connected to the module DbContext.");
    }
}
