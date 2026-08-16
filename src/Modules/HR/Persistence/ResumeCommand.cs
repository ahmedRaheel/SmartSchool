using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Write-side persistence for Resume.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ResumeCommand : IResumeCommand
{
    public Task AddAsync(
        Resume entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Resume create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Resume entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Resume update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Resume entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Resume delete persistence has not been connected to the module DbContext.");
    }
}
