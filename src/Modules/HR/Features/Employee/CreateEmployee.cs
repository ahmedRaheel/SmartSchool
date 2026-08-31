using SmartSchool.Modules.HR.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;
using SmartSchool.Application.Identity;

namespace SmartSchool.Modules.HR.Features.Employee;

public static class CreateEmployee
{
	public sealed record Response(
		Guid TenantId,
		Guid Id,
		Guid? UserId,
		string? EmployeeNumber,
		string FirstName,
		string? LastName,
		string? CnicNumber,
		DateOnly? DateOfBirth,
		string? Gender,
		string? JobTitle,
		byte[]? Photo,
		string? PhotoContentType,
		string? PhotoFileName,
		string? Email,
		string? Phone,
		string? AlternatePhone,
		string? Address,
		string? EmergencyContactName,
		string? EmergencyContactPhone,
		DateOnly HireDate,
		string EmploymentTypeCode,
		string StaffType,
		Guid? SourceCandidateId);

	public sealed record Request(
		Guid? TenantId,
		Guid SchoolId,
		Guid BranchId,
		Guid? DepartmentId,
		Guid? UserId,
		string FirstName,
		string? LastName,
		string? CnicNumber,
		DateOnly? DateOfBirth,
		string? Gender,
		string? JobTitle,
		byte[]? Photo,
		string? PhotoContentType,
		string? PhotoFileName,
		string? Email,
		string? Phone,
		string? AlternatePhone,
		string? Address,
		string? EmergencyContactName,
		string? EmergencyContactPhone,
		DateOnly HireDate,
		string EmploymentTypeCode,
		string StaffType,
		Guid? SourceCandidateId) : IRequest<Result<Response>>;

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.SchoolId).NotEmpty();
			RuleFor(x => x.BranchId).NotEmpty();
			RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
			RuleFor(x => x.EmploymentTypeCode).NotEmpty().MaximumLength(30);
			RuleFor(x => x.StaffType).Must(value => new[] { "TEACHER", "DRIVER", "PRINCIPAL", "ADMIN_OFFICER", "ACCOUNTANT", "HR", "LIBRARIAN", "TRANSPORT", "OTHER" }.Contains(value)).WithMessage("A valid staff type is required.");
		}
	}

	public interface ICreateEmployee
	{
		Task AddAsync(
				EmployeeEntity entity,
				CancellationToken cancellationToken);

		Task<bool> DepartmentBelongsToCampusAsync(Guid tenantId, Guid campusId, Guid departmentId, CancellationToken cancellationToken);

		Task<bool> CampusBelongsToSchoolAsync(Guid tenantId, Guid schoolId, Guid campusId, CancellationToken cancellationToken);

	}

	internal sealed class CreateEmployeePersistence(IHRDbContext dbContext) : ICreateEmployee
	{
		public async Task AddAsync(
				EmployeeEntity entity,
				CancellationToken cancellationToken)
			{
				await dbContext.Employees
					.AddAsync(entity, cancellationToken);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}

		public async Task<bool> CampusBelongsToSchoolAsync(Guid tenantId, Guid schoolId, Guid campusId, CancellationToken cancellationToken)
		{
			return await dbContext.Database.SqlQueryRaw<bool>(
				"SELECT EXISTS (SELECT 1 FROM org.campus WHERE tenant_id = {0} AND school_id = {1} AND campus_id = {2} AND is_active = TRUE) AS \"Value\"",
				tenantId, schoolId, campusId).SingleAsync(cancellationToken);
		}

		public async Task<bool> DepartmentBelongsToCampusAsync(Guid tenantId, Guid campusId, Guid departmentId, CancellationToken cancellationToken)
		{
			return await dbContext.Database.SqlQueryRaw<bool>(
				"SELECT EXISTS (SELECT 1 FROM org.department WHERE tenant_id = {0} AND campus_id = {1} AND department_id = {2} AND is_active = TRUE) AS \"Value\"",
				tenantId, campusId, departmentId).SingleAsync(cancellationToken);
		}
	}


	public sealed class Handler(ICreateEmployee persistence)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
		{
			var tenantId = request.TenantId!.Value;
			if (!await persistence.CampusBelongsToSchoolAsync(tenantId, request.SchoolId, request.BranchId, cancellationToken))
				return Result<Response>.Failure(Error.Validation("Selected branch does not belong to the selected school and tenant."));
			if (request.DepartmentId.HasValue && !await persistence.DepartmentBelongsToCampusAsync(tenantId, request.BranchId, request.DepartmentId.Value, cancellationToken))
				return Result<Response>.Failure(Error.Validation("Selected department does not belong to the selected branch."));

			var entity = EmployeeEntity.Create(
				tenantId, request.UserId, request.SchoolId, request.BranchId, request.DepartmentId,
				request.StaffType, null, request.FirstName, request.LastName, request.CnicNumber,
				request.DateOfBirth, request.Gender, request.JobTitle, request.Photo, request.PhotoContentType,
				request.PhotoFileName, request.Email, request.Phone, request.AlternatePhone, request.Address,
				request.EmergencyContactName, request.EmergencyContactPhone, request.HireDate,
				request.EmploymentTypeCode, LifecycleStatuses.PendingApproval, request.SourceCandidateId);

			await persistence.AddAsync(entity, cancellationToken);
			return Result<Response>.Success(new Response(entity.TenantId, entity.EmployeeId, entity.UserId,
				entity.EmployeeNumber, entity.FirstName, entity.LastName, entity.CnicNumber, entity.DateOfBirth,
				entity.Gender, entity.JobTitle, entity.Photo, entity.PhotoContentType, entity.PhotoFileName,
				entity.Email, entity.Phone, entity.AlternatePhone, entity.Address, entity.EmergencyContactName,
				entity.EmergencyContactPhone, entity.HireDate, entity.EmploymentTypeCode, entity.StaffType,
				entity.SourceCandidateId));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPost(
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "employee"),
				async (Request request, ITenantScope tenantScope, IMediator mediator, CancellationToken cancellationToken) =>
				{
                    var tenantId = tenantScope.Resolve(request.TenantId);
                    if (!tenantId.HasValue) return Results.BadRequest(new { message = "Tenant is required for SuperAdmin." });
                    request = request with { TenantId = tenantId.Value };
					var result = await mediator.SendAsync<Request, Result<Response>>(request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("CreateEmployee").WithTags(ModuleConstants.Name).RequireAuthorization();
		return endpoints;
	}

	
}
