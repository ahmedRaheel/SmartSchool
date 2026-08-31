using SmartSchool.Modules.Students.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;
using SmartSchool.Application.Identity;

namespace SmartSchool.Modules.Students.Features.Student;

public static class CreateStudent
{
	public sealed record Response(
		Guid TenantId,
		Guid Id,
		Guid? UserId,
		string? StudentNumber,
		string FirstName,
		string? LastName,
		DateOnly? DateOfBirth,
		string? Gender,
		byte[]? Photo,
		string? PhotoContentType,
		string? PhotoFileName,
		DateOnly? AdmissionDate,
		string Status);

	public sealed record Request(
		Guid? TenantId,
		Guid SchoolId,
		Guid BranchId,
		Guid AcademicYearId,
		Guid ClassSectionId,
		Guid? UserId,
		string FirstName,
		string? LastName,
		DateOnly? DateOfBirth,
		string? Gender,
		byte[]? Photo,
		string? PhotoContentType,
		string? PhotoFileName,
		DateOnly? AdmissionDate) : IRequest<Result<Response>>;

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.SchoolId).NotEmpty();
			RuleFor(x => x.BranchId).NotEmpty();
			RuleFor(x => x.AcademicYearId).NotEmpty();
			RuleFor(x => x.ClassSectionId).NotEmpty();
			RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
		}
	}

	public interface ICreateStudent
	{
		Task AddAsync(
				StudentEntity entity,
				CancellationToken cancellationToken);

		Task AddPlacementAsync(AdmissionPlacementEntity placement, CancellationToken cancellationToken);

		Task<bool> CampusBelongsToSchoolAsync(Guid tenantId, Guid schoolId, Guid campusId, CancellationToken cancellationToken);

	}

	internal sealed class CreateStudentPersistence(IStudentsDbContext dbContext) : ICreateStudent
	{
		public async Task AddAsync(
				StudentEntity entity,
				CancellationToken cancellationToken)
			{
				await dbContext.Students
					.AddAsync(entity, cancellationToken);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}

		public async Task AddPlacementAsync(AdmissionPlacementEntity placement, CancellationToken cancellationToken)
		    {
		        await dbContext.AdmissionPlacements.AddAsync(placement, cancellationToken);
		        await dbContext.SaveChangesAsync(cancellationToken);
		    }

		public async Task<bool> CampusBelongsToSchoolAsync(Guid tenantId, Guid schoolId, Guid campusId, CancellationToken cancellationToken)
		{
			return await dbContext.Database.SqlQueryRaw<bool>(
				"SELECT EXISTS (SELECT 1 FROM org.campus WHERE tenant_id = {0} AND school_id = {1} AND campus_id = {2} AND is_active = TRUE) AS \"Value\"",
				tenantId, schoolId, campusId).SingleAsync(cancellationToken);
		}

	}

	public sealed class Handler(ICreateStudent dataAccess)
		: IRequestHandler<Request, Result<Response>>
	{
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var tenantId = request.TenantId!.Value;
            var validScope = await dataAccess.CampusBelongsToSchoolAsync(
                tenantId,
                request.SchoolId,
                request.BranchId,
                cancellationToken);

            if (!validScope)
            {
                return Result<Response>.Failure(
                    Error.Validation("Selected branch does not belong to the selected school and tenant."));
            }

            var entity = StudentEntity.Create(
                tenantId,
                null,
                request.SchoolId,
                request.BranchId,
                null,
                request.FirstName,
                request.LastName,
                request.DateOfBirth,
                request.Gender,
                request.Photo,
                request.PhotoContentType,
                request.PhotoFileName,
                request.AdmissionDate,
                LifecycleStatuses.PendingApproval);

            await dataAccess.AddAsync(entity, cancellationToken);

            var placement = AdmissionPlacementEntity.Create(
                tenantId,
                entity.StudentId,
                request.AcademicYearId,
                request.ClassSectionId);

            await dataAccess.AddPlacementAsync(placement, cancellationToken);

            return Result<Response>.Success(MapResponse(entity));
        }
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPost(
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "student"),
				async (Request request,ITenantScope tenantScope, IMediator mediator, CancellationToken cancellationToken) =>
				{
                    var tenantId = tenantScope.Resolve(request.TenantId);
                    if (!tenantId.HasValue) return Results.BadRequest(new { message = "Tenant is required for SuperAdmin." });
                    request = request with { TenantId = tenantId.Value };
					var result = await mediator.SendAsync<Request, Result<Response>>(request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("CreateStudent").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantStudent);
		return endpoints;
	}

	private static Response MapResponse(StudentEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.StudentId,
			entity.UserId,
			entity.StudentNumber,
			entity.FirstName,
			entity.LastName,
			entity.DateOfBirth,
			entity.Gender,
			entity.Photo,
			entity.PhotoContentType,
			entity.PhotoFileName,
			entity.AdmissionDate,
			entity.Status);
	}
}
