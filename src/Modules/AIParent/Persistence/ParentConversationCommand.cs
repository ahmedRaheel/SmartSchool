using SmartSchool.Modules.AIParent.Models;

namespace SmartSchool.Modules.AIParent.Persistence;

/// <summary>
/// Write-side persistence for ParentConversationEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ParentConversationCommand : IParentConversationCommand
{
    public Task AddAsync(
        ParentConversationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentConversationEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ParentConversationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentConversationEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ParentConversationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentConversationEntity delete persistence has not been connected to the module DbContext.");
    }
}
