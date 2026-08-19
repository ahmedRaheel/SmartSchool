using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Persistence;

/// <summary>Executes tenant and recipient scoped notification reads.</summary>
public sealed class NotificationQuery(IApplicationDbContext dbContext) : INotificationQuery
{
	/// <inheritdoc />
	public Task<NotificationEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return dbContext.Set<NotificationEntity>().SingleOrDefaultAsync(
			entity => entity.TenantId == tenantId && entity.Id == id, cancellationToken);
	}

	/// <inheritdoc />
	public async Task<PagedResult<NotificationEntity>> GetPageAsync(Guid tenantId, Guid recipientUserId, int page, int pageSize, CancellationToken cancellationToken)
	{
		var query = dbContext.Set<NotificationEntity>().AsNoTracking()
			.Where(entity => entity.TenantId == tenantId && entity.RecipientUserId == recipientUserId);
		var totalCount = await query.LongCountAsync(cancellationToken);
		var items = await query.OrderByDescending(entity => entity.OccurredAt)
			.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
		return new PagedResult<NotificationEntity>(items, page, pageSize, totalCount);
	}

	/// <inheritdoc />
	public Task<int> GetUnreadCountAsync(Guid tenantId, Guid recipientUserId, CancellationToken cancellationToken)
	{
		return dbContext.Set<NotificationEntity>().AsNoTracking().CountAsync(
			entity => entity.TenantId == tenantId && entity.RecipientUserId == recipientUserId && !entity.IsRead, cancellationToken);
	}

	/// <inheritdoc />
	public async Task<IReadOnlyCollection<NotificationEntity>> GetUnreadAsync(Guid tenantId, Guid recipientUserId, CancellationToken cancellationToken)
	{
		return await dbContext.Set<NotificationEntity>()
			.Where(entity => entity.TenantId == tenantId && entity.RecipientUserId == recipientUserId && !entity.IsRead)
			.ToListAsync(cancellationToken);
	}
}
