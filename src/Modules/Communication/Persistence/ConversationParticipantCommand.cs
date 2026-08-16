using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence;

/// <summary>
/// Write-side persistence for ConversationParticipant.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ConversationParticipantCommand : IConversationParticipantCommand
{
    public Task AddAsync(
        ConversationParticipant entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ConversationParticipant create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ConversationParticipant entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ConversationParticipant update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ConversationParticipant entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ConversationParticipant delete persistence has not been connected to the module DbContext.");
    }
}
