using SmartSchool.Modules.AIParent.Models;

namespace SmartSchool.Modules.AIParent.Persistence;

public interface IParentConversationCommand
{
    Task AddAsync(
        ParentConversation entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        ParentConversation entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        ParentConversation entity,
        CancellationToken cancellationToken);
}
