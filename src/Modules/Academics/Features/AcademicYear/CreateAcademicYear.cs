using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.AcademicYear;

public static class CreateAcademicYear
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

	public sealed record Request(
		Guid TenantId,
		Guid CampusId,
		string Name,
		DateOnly StartDate,
		DateOnly EndDate,
		bool IsCurrent) : IRequest<Result<Response>>;

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.TenantId).NotEmpty();
			RuleFor(x => x.CampusId).NotEmpty();
			RuleFor(x => x.StartDate).NotEmpty();
			RuleFor(x => x.EndDate).NotEmpty().GreaterThan(x => x.StartDate);
			RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
		}
	}

	public interface ICreateAcademicYear
	{
		Task AddAsync(
				AcademicYearEntity entity,
				CancellationToken cancellationToken);

		Task<bool> CampusExistsAsync(
				Guid tenantId,
				Guid campusId,
				CancellationToken cancellationToken);
}

	internal sealed class CreateAcademicYearPersistence(IApplicationDbContext dbContext) : ICreateAcademicYear
	{
		public async Task AddAsync(AcademicYearEntity entity, CancellationToken cancellationToken)
		{
			await dbContext.Set<AcademicYearEntity>().AddAsync(entity, cancellationToken);
			await dbContext.SaveChangesAsync(cancellationToken);
		}

		public async Task<bool> CampusExistsAsync(Guid tenantId, Guid campusId, CancellationToken cancellationToken)
		{
			return await dbContext.Database.SqlQueryRaw<bool>(
				"SELECT EXISTS (SELECT 1 FROM org.campus WHERE tenant_id = {0} AND campus_id = {1} AND is_active = TRUE) AS \"Value\"",
				tenantId, campusId).SingleAsync(cancellationToken);
		}
	}

	public sealed class Handler(ICreateAcademicYear dataAccess)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(
			Request request,
			CancellationToken cancellationToken)
		{
			var campusExists = await dataAccess.CampusExistsAsync(
				request.TenantId, request.CampusId, cancellationToken);
			if (!campusExists)
			{
				return Result<Response>.Failure(
					Error.NotFound("The selected campus does not exist or is outside the tenant scope."));
			}


			var entity = AcademicYearEntity.Create(
				request.TenantId,
				request.CampusId,
				Guid.NewGuid().ToString("N").ToUpperInvariant(),
				request.Name,
				request.StartDate,
				request.EndDate,
				request.IsCurrent);

			await dataAccess.AddAsync(entity, cancellationToken);
			return Result<Response>.Success(MapResponse(entity));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPost(
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "academic-year"),
				async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var result = await mediator.SendAsync<Request, Result<Response>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("CreateAcademicYear")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantTeacher);
		return endpoints;
	}

	private static Response MapResponse(AcademicYearEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.AcademicYearId,
			entity.Code,
			entity.Name,
			entity.MetadataJson);
	}
}
