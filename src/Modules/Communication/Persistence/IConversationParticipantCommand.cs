using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence;

public interface IConversationParticipantCommand
{
    Task AddAsync(
        ConversationParticipant entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        ConversationParticipant entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        ConversationParticipant entity,
        CancellationToken cancellationToken);
}
