using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// Write-side persistence for TutorMessageEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class TutorMessageCommand : ITutorMessageCommand
{
    public Task AddAsync(
        TutorMessageEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TutorMessageEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        TutorMessageEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TutorMessageEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        TutorMessageEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TutorMessageEntity delete persistence has not been connected to the module DbContext.");
    }
}
