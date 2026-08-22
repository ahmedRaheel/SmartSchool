using Dapper;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Persistence;

/// <summary>
/// Provides optimized notification reads and command-side aggregate loading.
/// </summary>
public sealed class NotificationQuery(
	IApplicationDbContext dbContext,
	IDbConnectionFactory connectionFactory) : INotificationQuery
{
	public async Task<PagedResult<NotificationEntity>> GetPageAsync(
		Guid tenantId,
		Guid recipientUserId,
		int page,
		int pageSize,
		CancellationToken cancellationToken)
	{
		const string countSql = """
			SELECT COUNT(*)
			FROM communication.notification
			WHERE tenant_id = @TenantId
			  AND recipient_user_id = @RecipientUserId;
			""";

		const string pageSql = """
			SELECT
				notification_id AS "Id",
				tenant_id AS "TenantId",
				recipient_user_id AS "RecipientUserId",
				type AS "Type",
				title AS "Title",
				message AS "Message",
				related_entity_id AS "RelatedEntityId",
				related_entity_type AS "RelatedEntityType",
				action_url AS "ActionUrl",
				priority AS "Priority",
				is_read AS "IsRead",
				read_at AS "ReadAt",
				occurred_at AS "OccurredAt"
			FROM communication.notification
			WHERE tenant_id = @TenantId
			  AND recipient_user_id = @RecipientUserId
			ORDER BY occurred_at DESC
			LIMIT @PageSize OFFSET @Offset;
			""";

		await using var connection =
			await connectionFactory.OpenConnectionAsync(cancellationToken);

		var parameters = new
		{
			TenantId = tenantId,
			RecipientUserId = recipientUserId,
			PageSize = pageSize,
			Offset = (page - 1) * pageSize
		};

		var totalCount = await connection.ExecuteScalarAsync<long>(
			new CommandDefinition(
				countSql,
				parameters,
				cancellationToken: cancellationToken));

		var items = (await connection.QueryAsync<NotificationEntity>(
			new CommandDefinition(
				pageSql,
				parameters,
				cancellationToken: cancellationToken)))
			.AsList();

		return new PagedResult<NotificationEntity>(
			items,
			page,
			pageSize,
			totalCount);
	}

	public async Task<int> GetUnreadCountAsync(
		Guid tenantId,
		Guid recipientUserId,
		CancellationToken cancellationToken)
	{
		const string sql = """
			SELECT COUNT(*)
			FROM communication.notification
			WHERE tenant_id = @TenantId
			  AND recipient_user_id = @RecipientUserId
			  AND is_read = FALSE;
			""";

		await using var connection =
			await connectionFactory.OpenConnectionAsync(cancellationToken);

		return await connection.ExecuteScalarAsync<int>(
			new CommandDefinition(
				sql,
				new
				{
					TenantId = tenantId,
					RecipientUserId = recipientUserId
				},
				cancellationToken: cancellationToken));
	}

	public Task<NotificationEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken)
	{
		return dbContext
			.Set<NotificationEntity>()
			.SingleOrDefaultAsync(
				entity =>
					entity.TenantId == tenantId &&
					entity.Id == id,
				cancellationToken);
	}

	public Task<IReadOnlyCollection<NotificationEntity>> GetUnreadAsync(Guid tenantId, Guid recipientUserId, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}
