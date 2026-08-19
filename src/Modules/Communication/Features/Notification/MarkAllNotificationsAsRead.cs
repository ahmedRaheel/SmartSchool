using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Communication.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Communication.Features.Notification;

/// <summary>Marks all unread notifications for a recipient as read.</summary>
public static class MarkAllNotificationsAsRead
{
	public sealed record Command(Guid TenantId, Guid RecipientUserId) : IRequest<Result<Response>>;
	public sealed record Response(Guid TenantId, Guid RecipientUserId, int UpdatedCount);
	public sealed class Handler(INotificationQuery query, INotificationCommand command) : IRequestHandler<Command, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(Command request, CancellationToken cancellationToken)
		{
			var items = await query.GetUnreadAsync(request.TenantId, request.RecipientUserId, cancellationToken);
			foreach (var entity in items) { entity.MarkAsRead(); await command.UpdateAsync(entity, cancellationToken); }
			return Result<Response>.Success(new Response(request.TenantId, request.RecipientUserId, items.Count));
		}
	}
	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPatch(ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "notification") + "/read-all",
			async (Guid tenantId, Guid recipientUserId, IMediator mediator, CancellationToken cancellationToken) =>
			(await mediator.SendAsync<Command, Result<Response>>(new Command(tenantId, recipientUserId), cancellationToken)).ToHttpResult())
			.WithName("MarkAllNotificationsAsRead").WithTags(ModuleConstants.Name).RequireAuthorization();
		return endpoints;
	}
}
