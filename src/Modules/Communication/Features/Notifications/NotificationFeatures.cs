using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Features.Notifications;

/// <summary>Notification read port.</summary>
public interface INotificationQuery
{
    Task<PagedResult<Response>> GetPageAsync(Guid tenantId, Guid recipientUserId, int page, int pageSize, CancellationToken cancellationToken);
    Task<int> GetUnreadCountAsync(Guid tenantId, Guid recipientUserId, CancellationToken cancellationToken);
}
/// <summary>Notification write port.</summary>
public interface INotificationCommand
{
    Task AddAsync(NotificationEntity notification, CancellationToken cancellationToken);
    Task MarkAsReadAsync(Guid tenantId, Guid notificationId, Guid recipientUserId, CancellationToken cancellationToken);
}
/// <summary>Notification DTO.</summary>
public sealed record Response(Guid TenantId, Guid Id, NotificationType Type, string Title, string Message, Guid? RelatedEntityId, string? ActionUrl, bool IsRead, DateTimeOffset OccurredAt);
