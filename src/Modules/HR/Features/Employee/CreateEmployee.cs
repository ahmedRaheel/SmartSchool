using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.HR.Models;
using SmartSchool.Modules.HR.Persistence;
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

	public sealed class Handler(
        IEmployeeCommand entityCommand,
        IEmployeeOnboardingQuery onboardingQuery)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
		{
            var tenantId = request.TenantId!.Value;
            var validScope = await onboardingQuery.CampusBelongsToSchoolAsync(
                tenantId, request.SchoolId, request.BranchId, cancellationToken);
            if (!validScope)
            {
                return Result<Response>.Failure(
                    Error.Validation("Selected branch does not belong to the selected school and tenant."));
            }

            if (request.DepartmentId.HasValue &&
                !await onboardingQuery.DepartmentBelongsToCampusAsync(tenantId, request.BranchId, request.DepartmentId.Value, cancellationToken))
            {
                return Result<Response>.Failure(
                    Error.Validation("Selected department does not belong to the selected branch."));
            }

			var entity = EmployeeEntity.Create(
				request.TenantId!.Value,
				null,
				request.SchoolId,
				request.BranchId,
				request.DepartmentId,
				request.StaffType,
				null,
				request.FirstName,
				request.LastName,
				request.CnicNumber,
				request.Photo,
				request.PhotoContentType,
				request.PhotoFileName,
				request.Email,
				request.Phone,
				request.AlternatePhone,
				request.Address,
				request.EmergencyContactName,
				request.EmergencyContactPhone,
				request.HireDate,
				request.EmploymentTypeCode,
				LifecycleStatuses.Submitted,
				request.SourceCandidateId);

			await entityCommand.AddAsync(entity, cancellationToken);
			return Result<Response>.Success(MapResponse(entity));
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
