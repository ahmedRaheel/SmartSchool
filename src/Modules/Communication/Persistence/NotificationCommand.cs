using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence;

/// <summary>
/// Write-side persistence for NotificationEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class NotificationCommand : INotificationCommand
{
    public Task AddAsync(
        NotificationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "NotificationEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        NotificationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "NotificationEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        NotificationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "NotificationEntity delete persistence has not been connected to the module DbContext.");
    }
}
