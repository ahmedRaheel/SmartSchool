using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Persistence;

/// <summary>Executes tenant and recipient scoped notification reads.</summary>
public sealed class NotificationQuery(
	IApplicationDbContext dbContext,
	IDapperReadStore dapperReadStore) : INotificationQuery
{
	/// <inheritdoc />
	public Task<NotificationEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return dbContext.Set<NotificationEntity>().SingleOrDefaultAsync(
			entity => entity.TenantId == tenantId && entity.Id == id, cancellationToken);
	}

	/// <inheritdoc />
	public Task<PagedResult<NotificationEntity>> GetPageAsync(
		Guid tenantId,
		Guid recipientUserId,
		int page,
		int pageSize,
		CancellationToken cancellationToken)
	{
		return dapperReadStore.GetFilteredPageAsync<NotificationEntity>(
			tenantId,
			page,
			pageSize,
			[
				nameof(Entity.Id),
				nameof(Entity.TenantId),
				nameof(NotificationEntity.RecipientUserId),
				nameof(NotificationEntity.Type),
				nameof(NotificationEntity.Title),
				nameof(NotificationEntity.Message),
				nameof(NotificationEntity.RelatedEntityId),
				nameof(NotificationEntity.RelatedEntityType),
				nameof(NotificationEntity.ActionUrl),
				nameof(NotificationEntity.Priority),
				nameof(NotificationEntity.IsRead),
				nameof(NotificationEntity.ReadAt),
				nameof(NotificationEntity.OccurredAt)
			],
			new Dictionary<string, object?>
			{
				[nameof(NotificationEntity.RecipientUserId)] = recipientUserId
			},
			nameof(NotificationEntity.OccurredAt),
			descending: true,
			cancellationToken);
	}

	/// <inheritdoc />
	public Task<int> GetUnreadCountAsync(Guid tenantId, Guid recipientUserId, CancellationToken cancellationToken)
	{
		return dapperReadStore.CountAsync<NotificationEntity>(
			tenantId,
			new Dictionary<string, object?>
			{
				[nameof(NotificationEntity.RecipientUserId)] = recipientUserId,
				[nameof(NotificationEntity.IsRead)] = false
			},
			cancellationToken);
	}

	/// <inheritdoc />
	public async Task<IReadOnlyCollection<NotificationEntity>> GetUnreadAsync(Guid tenantId, Guid recipientUserId, CancellationToken cancellationToken)
	{
		return await dbContext.Set<NotificationEntity>()
			.Where(entity => entity.TenantId == tenantId && entity.RecipientUserId == recipientUserId && !entity.IsRead)
			.ToListAsync(cancellationToken);
	}
}
