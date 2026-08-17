using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence;

/// <summary>
/// Write-side persistence for ConversationEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ConversationCommand : IConversationCommand
{
    public Task AddAsync(
        ConversationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ConversationEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ConversationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ConversationEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ConversationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ConversationEntity delete persistence has not been connected to the module DbContext.");
    }
}
