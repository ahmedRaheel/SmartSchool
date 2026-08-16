using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Write-side persistence for Interview.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class InterviewCommand : IInterviewCommand
{
    public Task AddAsync(
        Interview entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Interview create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Interview entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Interview update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Interview entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Interview delete persistence has not been connected to the module DbContext.");
    }
}
