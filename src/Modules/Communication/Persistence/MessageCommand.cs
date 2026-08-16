using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence;

/// <summary>
/// Write-side persistence for Message.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class MessageCommand : IMessageCommand
{
    public Task AddAsync(
        Message entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Message create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Message entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Message update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Message entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Message delete persistence has not been connected to the module DbContext.");
    }
}
