using SmartSchool.Modules.Communication.Models;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Communication.Features.Notification;

/// <summary>Marks one recipient-owned notification as read.</summary>
public static class MarkNotificationAsRead
{
	public sealed record Command(Guid TenantId, Guid Id, Guid RecipientUserId) : IRequest<Result<Response>>;
	public sealed record Response(Guid TenantId, Guid Id, bool IsRead, DateTimeOffset? ReadAt);

	public interface IMarkNotificationAsRead
	{
		Task<NotificationEntity?> GetByIdAsync(
			Guid tenantId,
			Guid id,
			CancellationToken cancellationToken);

		Task UpdateAsync(
				NotificationEntity entity,
				CancellationToken cancellationToken);

	}

	internal sealed class MarkNotificationAsReadPersistence(
		IApplicationDbContext dbContext) : IMarkNotificationAsRead
	{
		public Task<NotificationEntity?> GetByIdAsync(
			Guid tenantId,
			Guid id,
			CancellationToken cancellationToken)
		{
			return dbContext
				.Set<NotificationEntity>()
				.SingleOrDefaultAsync(
					entity => entity.TenantId == tenantId && entity.NotificationId == id,
					cancellationToken);
		}

		public async Task UpdateAsync(
				NotificationEntity entity,
				CancellationToken cancellationToken)
			{
				dbContext
					.Set<NotificationEntity>()
					.Update(entity);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}
	}

	public sealed class Handler(IMarkNotificationAsRead dataAccess) : IRequestHandler<Command, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(Command request, CancellationToken cancellationToken)
		{
			var entity = await dataAccess.GetByIdAsync(request.TenantId, request.Id, cancellationToken);
			if (entity is null || entity.RecipientUserId != request.RecipientUserId)
				return Result<Response>.Failure(Error.NotFound(ErrorMessages.EntityNotFound("Notification")));
			entity.MarkAsRead();
			await dataAccess.UpdateAsync(entity, cancellationToken);
			return Result<Response>.Success(new Response(entity.TenantId, entity.NotificationId, entity.IsRead, entity.ReadAt));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPatch(ApiRoutes.EntityById(ModuleConstants.RouteSegment, "notification") + "/read",
			async (Guid id, Guid tenantId, Guid recipientUserId, IMediator mediator, CancellationToken cancellationToken) =>
			(await mediator.SendAsync<Command, Result<Response>>(new Command(tenantId, id, recipientUserId), cancellationToken)).ToHttpResult())
			.WithName("MarkNotificationAsRead").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.AllAuthenticatedActors);
		return endpoints;
	}
}
