using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence;

public interface INotificationCommand
{
    Task AddAsync(
        Notification entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Notification entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Notification entity,
        CancellationToken cancellationToken);
}
