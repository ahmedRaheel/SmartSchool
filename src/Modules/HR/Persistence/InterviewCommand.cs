using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Write-side persistence for InterviewEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class InterviewCommand : IInterviewCommand
{
    public Task AddAsync(
        InterviewEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InterviewEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        InterviewEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InterviewEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        InterviewEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InterviewEntity delete persistence has not been connected to the module DbContext.");
    }
}
