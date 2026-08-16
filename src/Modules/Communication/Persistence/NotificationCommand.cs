using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence;

/// <summary>
/// Write-side persistence for Notification.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class NotificationCommand : INotificationCommand
{
    public Task AddAsync(
        Notification entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Notification create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Notification entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Notification update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Notification entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Notification delete persistence has not been connected to the module DbContext.");
    }
}
