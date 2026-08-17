using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// Write-side persistence for TutorConversationEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class TutorConversationCommand : ITutorConversationCommand
{
    public Task AddAsync(
        TutorConversationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TutorConversationEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        TutorConversationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TutorConversationEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        TutorConversationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TutorConversationEntity delete persistence has not been connected to the module DbContext.");
    }
}
