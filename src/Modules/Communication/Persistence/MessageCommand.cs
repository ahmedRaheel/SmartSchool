using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence;

/// <summary>
/// Write-side persistence for MessageEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class MessageCommand : IMessageCommand
{
    public Task AddAsync(
        MessageEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "MessageEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        MessageEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "MessageEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        MessageEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "MessageEntity delete persistence has not been connected to the module DbContext.");
    }
}
