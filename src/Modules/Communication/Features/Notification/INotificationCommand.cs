using System.Threading.Tasks;
using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Features.Notification;

/// <summary>
/// Defines command persistence operations for NotificationEntity.
/// </summary>
public interface INotificationCommand
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task AddAsync(
        NotificationEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task UpdateAsync(
        NotificationEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task DeleteAsync(
        NotificationEntity entity,
        CancellationToken cancellationToken);
}
