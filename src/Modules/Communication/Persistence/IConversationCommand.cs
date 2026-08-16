using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence;

public interface IConversationCommand
{
    Task AddAsync(
        Conversation entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Conversation entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Conversation entity,
        CancellationToken cancellationToken);
}
