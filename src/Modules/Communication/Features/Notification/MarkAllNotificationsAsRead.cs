using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Communication.Features.Notification;

/// <summary>Marks all unread notifications for a recipient as read.</summary>
public static class MarkAllNotificationsAsRead
{
	public sealed record Command(Guid TenantId, Guid RecipientUserId) : IRequest<Result<Response>>;
	public sealed record Response(Guid TenantId, Guid RecipientUserId, int UpdatedCount);
	public interface IMarkAllNotificationsAsRead
	{
		Task UpdateAsync(
				NotificationEntity entity,
				CancellationToken cancellationToken);

		Task<IReadOnlyCollection<NotificationEntity>> GetUnreadAsync(
				Guid tenantId,
				Guid recipientUserId,
				CancellationToken cancellationToken);

	}

	internal sealed class MarkAllNotificationsAsReadDataAccess(
		IApplicationDbContext dbContext,
		IDbConnectionFactory connectionFactory) : IMarkAllNotificationsAsRead
	{
		public async Task UpdateAsync(
				NotificationEntity entity,
				CancellationToken cancellationToken)
			{
				dbContext
					.Set<NotificationEntity>()
					.Update(entity);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}

		public async Task<IReadOnlyCollection<NotificationEntity>> GetUnreadAsync(
				Guid tenantId,
				Guid recipientUserId,
				CancellationToken cancellationToken)
			{
				return await dbContext
					.Set<NotificationEntity>()
					.Where(entity =>
						entity.TenantId == tenantId &&
						entity.RecipientUserId == recipientUserId &&
						!entity.IsRead &&
						entity.IsActive)
					.OrderByDescending(entity => entity.OccurredAt)
					.ToListAsync(cancellationToken);
			}
	}

	public sealed class Handler(IMarkAllNotificationsAsRead dataAccess) : IRequestHandler<Command, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(Command request, CancellationToken cancellationToken)
		{
			var items = await dataAccess.GetUnreadAsync(request.TenantId, request.RecipientUserId, cancellationToken);
			foreach (var entity in items) { entity.MarkAsRead(); await dataAccess.UpdateAsync(entity, cancellationToken); }
			return Result<Response>.Success(new Response(request.TenantId, request.RecipientUserId, items.Count));
		}
	}
	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPatch(ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "notification") + "/read-all",
			async (Guid tenantId, Guid recipientUserId, IMediator mediator, CancellationToken cancellationToken) =>
			(await mediator.SendAsync<Command, Result<Response>>(new Command(tenantId, recipientUserId), cancellationToken)).ToHttpResult())
			.WithName("MarkAllNotificationsAsRead").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.AllAuthenticatedActors);
		return endpoints;
	}
}
