using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence;

/// <summary>
/// Write-side persistence for MessageReceiptEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class MessageReceiptCommand : IMessageReceiptCommand
{
    public Task AddAsync(
        MessageReceiptEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "MessageReceiptEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        MessageReceiptEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "MessageReceiptEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        MessageReceiptEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "MessageReceiptEntity delete persistence has not been connected to the module DbContext.");
    }
}
