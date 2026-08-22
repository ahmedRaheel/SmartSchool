using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.Modules.Communication.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Communication.Features.Notification;

public static class UpdateNotification
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
		Guid Id,
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
			RuleFor(x => x.Id).NotEmpty();
			RuleFor(x => x.Type).IsInEnum();
			RuleFor(x => x.Title).NotEmpty().MaximumLength(250);
			RuleFor(x => x.Message).NotEmpty().MaximumLength(2000);
			RuleFor(x => x.Priority).NotEmpty().MaximumLength(50);
		}
	}

	public sealed class Handler(
		INotificationQuery entityQuery,
		INotificationCommand entityCommand)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(
			Request request,
			CancellationToken cancellationToken)
		{
			var entity = await entityQuery.GetByIdAsync(
				request.TenantId, request.Id, cancellationToken);
			if (entity is null)
			{
				return Result<Response>.Failure(
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(NotificationEntity))));
			}


			entity.UpdateDetails(
				request.Type,
				request.Title,
				request.Message,
				request.RelatedEntityId,
				request.RelatedEntityType,
				request.ActionUrl,
				request.Priority);
			await entityCommand.UpdateAsync(entity, cancellationToken);
			return Result<Response>.Success(MapResponse(entity));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPut(
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "notification"),
				async (Guid id, Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var command = request with { Id = id };
					var result = await mediator.SendAsync<Request, Result<Response>>(
						command, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("UpdateNotification")
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
