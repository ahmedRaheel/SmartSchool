using SmartSchool.Modules.Communication.Realtime;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.Modules.Communication.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Communication.Features.Notification;

public static class CreateNotification
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

	public sealed record Request(
		Guid TenantId,
		Guid RecipientUserId,
		NotificationType Type,
		string Title,
		string Message,
		Guid? RelatedEntityId,
		string? RelatedEntityType,
		string? ActionUrl,
		string Priority) : IRequest<Result<Response>>;

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.TenantId).NotEmpty();
			RuleFor(x => x.Type).IsInEnum();
			RuleFor(x => x.Title).NotEmpty().MaximumLength(250);
			RuleFor(x => x.Message).NotEmpty().MaximumLength(2000);
			RuleFor(x => x.Priority).NotEmpty().MaximumLength(50);
		}
	}

	public sealed class Handler(
		INotificationCommand entityCommand,
		IHubContext<NotificationHub> notificationHub)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(
			Request request,
			CancellationToken cancellationToken)
		{

			var entity = NotificationEntity.Create(
					request.TenantId,
					request.RecipientUserId,
					request.Type,
					request.Title,
					request.Message,
					request.RelatedEntityId,
					request.RelatedEntityType,
					request.ActionUrl,
					request.Priority);
			await entityCommand.AddAsync(entity, cancellationToken);
			var response = MapResponse(entity);
			await notificationHub.Clients
				.Group(CommunicationGroups.User(entity.TenantId, entity.RecipientUserId))
				.SendAsync("NotificationReceived", response, cancellationToken);
			return Result<Response>.Success(response);
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPost(
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "notification"),
				async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var result = await mediator.SendAsync<Request, Result<Response>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("CreateNotification")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantAdmin);
		return endpoints;
	}

	private static Response MapResponse(
		NotificationEntity entity)
	{
		return new Response(
		entity.TenantId,
			entity.Id,
			entity.RecipientUserId,
			entity.Type,
			entity.Title,
			entity.Message,
			entity.RelatedEntityId,
			entity.RelatedEntityType,
			entity.ActionUrl,
			entity.Priority,
			entity.IsRead,
			entity.ReadAt,
			entity.OccurredAt);
	}
}
