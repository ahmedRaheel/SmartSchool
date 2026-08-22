using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Students.Models;
using SmartSchool.Modules.Students.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Features.Guardian;

public static class GetGuardianById
{
	public sealed record Response(
		Guid TenantId,
		Guid Id,
		Guid? UserId,
		string FullName,
		string? CnicNumber,
		string? Email,
		string? Phone);

	public sealed record Query(Guid TenantId, Guid Id) : IRequest<Result<Response>>;

	public sealed class Handler(IGuardianQuery entityQuery) : IRequestHandler<Query, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(Query request, CancellationToken cancellationToken)
		{
			var entity = await entityQuery.GetByIdAsync(request.TenantId, request.Id, cancellationToken);
			if (entity is null)
			{
				return Result<Response>.Failure(
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(GuardianEntity))));
			}
			return Result<Response>.Success(MapResponse(entity));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapGet(
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "guardian"),
				async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var result = await mediator.SendAsync<Query, Result<Response>>(new Query(tenantId, id), cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("GetGuardianById").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantStudent);
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
