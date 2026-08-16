using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// Write-side persistence for TutorMessage.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class TutorMessageCommand : ITutorMessageCommand
{
    public Task AddAsync(
        TutorMessage entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TutorMessage create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        TutorMessage entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TutorMessage update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        TutorMessage entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TutorMessage delete persistence has not been connected to the module DbContext.");
    }
}
