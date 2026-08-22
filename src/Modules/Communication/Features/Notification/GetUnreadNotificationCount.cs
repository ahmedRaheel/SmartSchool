using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Communication.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Communication.Features.Notification;

/// <summary>Gets the unread notification badge count.</summary>
public static class GetUnreadNotificationCount
{
	public sealed record Query(Guid TenantId, Guid RecipientUserId) : IRequest<Result<Response>>;
	public sealed record Response(Guid TenantId, Guid RecipientUserId, int UnreadCount);
	public sealed class Handler(INotificationQuery query) : IRequestHandler<Query, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(Query request, CancellationToken cancellationToken)
		{
			var count = await query.GetUnreadCountAsync(request.TenantId, request.RecipientUserId, cancellationToken);
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
