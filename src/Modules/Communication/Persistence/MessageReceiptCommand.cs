using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence;

/// <summary>
/// Write-side persistence for MessageReceipt.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class MessageReceiptCommand : IMessageReceiptCommand
{
    public Task AddAsync(
        MessageReceipt entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "MessageReceipt create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        MessageReceipt entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "MessageReceipt update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        MessageReceipt entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "MessageReceipt delete persistence has not been connected to the module DbContext.");
    }
}
