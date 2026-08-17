using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence;

/// <summary>
/// Write-side persistence for ConversationParticipantEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ConversationParticipantCommand : IConversationParticipantCommand
{
    public Task AddAsync(
        ConversationParticipantEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ConversationParticipantEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ConversationParticipantEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ConversationParticipantEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ConversationParticipantEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ConversationParticipantEntity delete persistence has not been connected to the module DbContext.");
    }
}
