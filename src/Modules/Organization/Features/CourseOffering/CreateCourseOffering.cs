using Microsoft.EntityFrameworkCore;
using SmartSchool.Modules.Organization.Persistence;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Organization.Features.CourseOffering;

public static class CreateCourseOffering
{
	/// <summary>
	/// Represents the response returned by this CourseOfferingEntity feature.
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
        Guid BranchId,
		string Name) : IRequest<Result<Response>>;

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.TenantId).NotEmpty();
			RuleFor(x => x.BranchId).NotEmpty();
			RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
		}
	}

	public interface ICreateCourseOffering
	{
		Task AddAsync(
				CourseOfferingEntity entity,
				CancellationToken cancellationToken);

	}

	internal sealed class CreateCourseOfferingPersistence(
		IOrganizationDbContext dbContext) : ICreateCourseOffering
	{
		public async Task AddAsync(
				CourseOfferingEntity entity,
				CancellationToken cancellationToken)
			{
				await dbContext
					.CourseOfferings
					.AddAsync(entity, cancellationToken);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}
	}

	public sealed class Handler(IBusinessNumberGenerator numberGenerator,
		IOrganizationDbContext dbContext,
		ICreateCourseOffering dataAccess)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(
			Request request,
			CancellationToken cancellationToken)
		{
            var branchCode = await dbContext.Database
                .SqlQueryRaw<string>(
                    "SELECT code AS \"Value\" FROM org.campus WHERE tenant_id = {0} AND campus_id = {1} AND is_active = TRUE",
                    request.TenantId,
                    request.BranchId)
                .SingleOrDefaultAsync(cancellationToken);
            if (string.IsNullOrWhiteSpace(branchCode)) return Result<Response>.Failure(Error.Validation("A valid branch is required."));
            var code = await numberGenerator.NextAsync("COURSE:" + request.BranchId, $"{branchCode}-CR-", request.TenantId, 5, cancellationToken);

			var entity = CourseOfferingEntity.Create(
				request.TenantId,
                request.BranchId,
				code,
				request.Name);

			await dataAccess.AddAsync(entity, cancellationToken);
			return Result<Response>.Success(MapResponse(entity));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPost(
				ApiRoutes.EntityCollection("academics", "course-offering"),
				async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var result = await mediator.SendAsync<Request, Result<Response>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("CreateCourseOffering")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantTeacher);
		return endpoints;
	}

	private static Response MapResponse(
		CourseOfferingEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.CourseOfferingId,
			entity.Code,
			entity.Name,
			entity.MetadataJson);
	}
}
