using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Persistence;

/// <summary>
/// EF-backed read persistence for NotificationEntity.
/// </summary>
public sealed class NotificationQuery(IEfMockStore store) : INotificationQuery
{
	public Task<NotificationEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<NotificationEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<NotificationEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<NotificationEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<NotificationEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
