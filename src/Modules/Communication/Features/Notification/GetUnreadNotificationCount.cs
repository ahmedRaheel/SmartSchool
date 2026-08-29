using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Communication.Features.Notification;

/// <summary>Gets the unread notification badge count.</summary>
public static class GetUnreadNotificationCount
{
	public sealed record Query(Guid TenantId, Guid RecipientUserId) : IRequest<Result<Response>>;
	public sealed record Response(Guid TenantId, Guid RecipientUserId, int UnreadCount);
	public interface IGetUnreadNotificationCount
	{
		Task<int> GetUnreadCountAsync(
				Guid tenantId,
				Guid recipientUserId,
				CancellationToken cancellationToken);

	}

	internal sealed class GetUnreadNotificationCountDataAccess(
		IDbConnectionFactory connectionFactory) : IGetUnreadNotificationCount
	{
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
	}

	public sealed class Handler(IGetUnreadNotificationCount dataAccess) : IRequestHandler<Query, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(Query request, CancellationToken cancellationToken)
		{
			var count = await dataAccess.GetUnreadCountAsync(request.TenantId, request.RecipientUserId, cancellationToken);
			return Result<Response>.Success(new Response(request.TenantId, request.RecipientUserId, count));
		}
	}
	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapGet(ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "notification") + "/unread-count",
			async (Guid tenantId, Guid recipientUserId, IMediator mediator, CancellationToken cancellationToken) =>
			(await mediator.SendAsync<Query, Result<Response>>(new Query(tenantId, recipientUserId), cancellationToken)).ToHttpResult())
			.WithName("GetUnreadNotificationCount").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.AllAuthenticatedActors);
		return endpoints;
	}
}
