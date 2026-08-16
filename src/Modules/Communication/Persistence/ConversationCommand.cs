using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence;

/// <summary>
/// Write-side persistence for Conversation.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ConversationCommand : IConversationCommand
{
    public Task AddAsync(
        Conversation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Conversation create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Conversation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Conversation update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Conversation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Conversation delete persistence has not been connected to the module DbContext.");
    }
}
