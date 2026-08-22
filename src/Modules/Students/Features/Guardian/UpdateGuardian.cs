using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Students.Models;
using SmartSchool.Modules.Students.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Features.Guardian;

public static class UpdateGuardian
{
	public sealed record Response(
		Guid TenantId,
		Guid Id,
		Guid? UserId,
		string FullName,
		string? CnicNumber,
		string? Email,
		string? Phone);

	public sealed record Request(
		Guid TenantId,
		Guid Id,
		string FullName,
		string? CnicNumber,
		string? Email,
		string? Phone) : IRequest<Result<Response>>;

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.TenantId).NotEmpty();
			RuleFor(x => x.Id).NotEmpty();
		}
	}

	public sealed class Handler(IGuardianQuery entityQuery, IGuardianCommand entityCommand)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
		{
			var entity = await entityQuery.GetByIdAsync(request.TenantId, request.Id, cancellationToken);
			if (entity is null)
			{
				return Result<Response>.Failure(
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(GuardianEntity))));
			}

			entity.UpdateDetails(
				request.FullName,
				request.CnicNumber,
				request.Email,
				request.Phone);

			await entityCommand.UpdateAsync(entity, cancellationToken);
			return Result<Response>.Success(MapResponse(entity));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPut(
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "guardian"),
				async (Guid id, Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var command = request with { Id = id };
					var result = await mediator.SendAsync<Request, Result<Response>>(command, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("UpdateGuardian").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantStudent);
		return endpoints;
	}

	private static Response MapResponse(GuardianEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.Id,
			entity.UserId,
			entity.FullName,
			entity.CnicNumber,
			entity.Email,
			entity.Phone);
	}
}
