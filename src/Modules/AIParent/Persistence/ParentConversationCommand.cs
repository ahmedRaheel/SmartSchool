using SmartSchool.Modules.AIParent.Models;

namespace SmartSchool.Modules.AIParent.Persistence;

/// <summary>
/// Write-side persistence for ParentConversation.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ParentConversationCommand : IParentConversationCommand
{
    public Task AddAsync(
        ParentConversation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentConversation create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ParentConversation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentConversation update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ParentConversation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentConversation delete persistence has not been connected to the module DbContext.");
    }
}
