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

	internal sealed class CreateEmployeePersistence(IApplicationDbContext dbContext) : ICreateEmployee
	{
		public async Task AddAsync(
				EmployeeEntity entity,
				CancellationToken cancellationToken)
			{
				await dbContext
					.Set<EmployeeEntity>()
					.AddAsync(entity, cancellationToken);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}

		public async Task<bool> DepartmentBelongsToCampusAsync(Guid tenantId, Guid campusId, Guid departmentId, CancellationToken cancellationToken)
		{
			return await dbContext.Database.SqlQueryRaw<bool>(
				"SELECT EXISTS (SELECT 1 FROM org.department WHERE tenant_id = {0} AND campus_id = {1} AND department_id = {2} AND is_active = TRUE) AS \"Value\"",
				tenantId, campusId, departmentId).SingleAsync(cancellationToken);
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

	private static Response MapResponse(EmployeeEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.EmployeeId,
			entity.UserId,
			entity.EmployeeNumber,
			entity.FirstName,
			entity.LastName,
			entity.CnicNumber,
			entity.Photo,
			entity.PhotoContentType,
			entity.PhotoFileName,
			entity.Email,
			entity.Phone,
			entity.AlternatePhone,
			entity.Address,
			entity.EmergencyContactName,
			entity.EmergencyContactPhone,
			entity.HireDate,
			entity.EmploymentTypeCode,
			entity.StaffType,
			entity.SourceCandidateId);
	}
}
