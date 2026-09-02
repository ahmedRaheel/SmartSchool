using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Communication.Features.Notification;

public static class GetNotificationPage
{
	/// <summary>
	/// Represents the response returned by this NotificationEntity feature.
	/// </summary>
	/// <param name="TenantId">The owning tenant identifier.</param>
	/// <param name="Id">The entity identifier.</param>
			public sealed record Response(
	Guid TenantId,
	Guid Id,
	Guid RecipientUserId,
	NotificationType Type,
	string Title,
	string Message,
	Guid? RelatedEntityId,
	string? RelatedEntityType,
	string? ActionUrl,
	string Priority,
	bool IsRead,
	DateTimeOffset? ReadAt,
	DateTimeOffset OccurredAt);

	public sealed record Query(
		Guid? TenantId,
		Guid RecipientUserId,
		int Page = 1,
		int PageSize = 25) : IRequest<Result<PagedResult<Response>>>;

	public interface IGetNotificationPage
	{
		Task<PagedResult<Response>> GetPageAsync(
				Guid? tenantId,
				Guid recipientUserId,
				int page,
				int pageSize,
				CancellationToken cancellationToken);

	}

	internal sealed class GetNotificationPagePersistence(		
		IDbConnectionFactory connectionFactory) : IGetNotificationPage
	{
		public async Task<PagedResult<Response>> GetPageAsync(
				Guid? tenantId,
				Guid recipientUserId,
				int page,
				int pageSize,
				CancellationToken cancellationToken)
			{
				const string countSql = """
					SELECT COUNT(*)
					FROM communication.notification
					WHERE (@TenantId IS NULL OR tenant_id = @TenantId)
					  AND recipient_user_id = @RecipientUserId;
					""";
		
				const string pageSql = """
					SELECT
					tenant_id AS "TenantId",
					notification_id AS "Id",
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
					WHERE (@TenantId IS NULL OR tenant_id = @TenantId)
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
		
				var items = (await connection.QueryAsync<Response>(
					new CommandDefinition(
						pageSql,
						parameters,
						cancellationToken: cancellationToken)))
					.AsList();
		
				return new PagedResult<Response>(
					items,
					page,
					pageSize,
					totalCount);
			}
	}

	public sealed class Handler(IGetNotificationPage dataAccess)
		: IRequestHandler<Query, Result<PagedResult<Response>>>
	{
		public async Task<Result<PagedResult<Response>>> HandleAsync(
			Query request,
			CancellationToken cancellationToken)
		{
			var pageRequest = new PageRequest(request.Page, request.PageSize);
			var page = await dataAccess.GetPageAsync(
				request.TenantId,
				request.RecipientUserId,
				pageRequest.NormalizedPage,
				pageRequest.NormalizedPageSize,
				cancellationToken);
			var response = new PagedResult<Response>(
				page.Items,
				page.Page,
				page.PageSize,
				page.TotalCount);
			return Result<PagedResult<Response>>.Success(response);
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapGet(
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "notification"),
				async (Guid? tenantId, Guid recipientUserId, int page, int pageSize, SmartSchool.Application.Identity.ITenantScope tenantScope, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var effectiveTenantId = tenantScope.Resolve(tenantId);
                    if (!tenantScope.IsSuperAdmin && recipientUserId != tenantScope.UserId) return Results.Forbid();
                    var request = new Query(effectiveTenantId, recipientUserId, page, pageSize);
					var result = await mediator.SendAsync<Query, Result<PagedResult<Response>>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("GetNotificationPage")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization(SmartSchoolPolicies.AllAuthenticatedActors);
		return endpoints;
	}
}
