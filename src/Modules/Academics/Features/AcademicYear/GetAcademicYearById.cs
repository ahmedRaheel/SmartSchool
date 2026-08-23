using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.Modules.Academics.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.AcademicYear;

public static class GetAcademicYearById
{
	/// <summary>
	/// Represents the response returned by this AcademicYearEntity feature.
	/// </summary>
	/// <param name="TenantId">The owning tenant identifier.</param>
	/// <param name="Id">The entity identifier.</param>
	/// <param name="Code">The business code.</param>
	/// <param name="Name">The display name.</param>
	public sealed record Response(
	Guid TenantId,
	Guid Id,
	string Code,
	string Name,
	string? MetadataJson);

	public sealed record Query(
		Guid TenantId,
		Guid Id) : IRequest<Result<Response>>;

	public sealed class Handler(IAcademicYearQuery entityQuery)
		: IRequestHandler<Query, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(
			Query request,
			CancellationToken cancellationToken)
		{
			var entity = await entityQuery.GetByIdAsync(
				request.TenantId, request.Id, cancellationToken);
			if (entity is null)
			{
				return Result<Response>.Failure(
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(AcademicYearEntity))));
			}
			return Result<Response>.Success(MapResponse(entity));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapGet(
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "academic-year"),
				async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var request = new Query(tenantId, id);
					var result = await mediator.SendAsync<Query, Result<Response>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("GetAcademicYearById")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantTeacher);
		return endpoints;
	}

	private static Response MapResponse(
		AcademicYearEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.Id,
			entity.Code,
			entity.Name,
			entity.MetadataJson);
	}
}
