using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Write-side persistence for ResumeEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ResumeCommand : IResumeCommand
{
    public Task AddAsync(
        ResumeEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ResumeEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ResumeEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ResumeEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ResumeEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ResumeEntity delete persistence has not been connected to the module DbContext.");
    }
}
