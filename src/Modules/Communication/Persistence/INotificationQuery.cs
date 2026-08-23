using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Persistence;

/// <summary>Defines tenant-scoped notification read operations.</summary>
public interface INotificationQuery
{
	/// <summary>Gets a notification by identifier.</summary>
	Task<NotificationEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken);

	/// <summary>Gets a recipient's notifications ordered newest first.</summary>
	Task<PagedResult<NotificationEntity>> GetPageAsync(Guid? tenantId, Guid recipientUserId, int page, int pageSize, CancellationToken cancellationToken);

	/// <summary>Gets the unread notification count for a recipient.</summary>
	Task<int> GetUnreadCountAsync(Guid tenantId, Guid recipientUserId, CancellationToken cancellationToken);

	/// <summary>Gets all unread notifications for a recipient.</summary>
	Task<IReadOnlyCollection<NotificationEntity>> GetUnreadAsync(Guid tenantId, Guid recipientUserId, CancellationToken cancellationToken);
}
