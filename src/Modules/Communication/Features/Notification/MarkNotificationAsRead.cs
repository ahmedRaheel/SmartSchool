using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Communication.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Communication.Features.Notification;

/// <summary>Marks one recipient-owned notification as read.</summary>
public static class MarkNotificationAsRead
{
	public sealed record Command(Guid TenantId, Guid Id, Guid RecipientUserId) : IRequest<Result<Response>>;
	public sealed record Response(Guid TenantId, Guid Id, bool IsRead, DateTimeOffset? ReadAt);

	public sealed class Handler(INotificationQuery query, INotificationCommand command) : IRequestHandler<Command, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(Command request, CancellationToken cancellationToken)
		{
			var entity = await query.GetByIdAsync(request.TenantId, request.Id, cancellationToken);
			if (entity is null || entity.RecipientUserId != request.RecipientUserId)
				return Result<Response>.Failure(Error.NotFound(ErrorMessages.EntityNotFound("Notification")));
			entity.MarkAsRead();
			await command.UpdateAsync(entity, cancellationToken);
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
