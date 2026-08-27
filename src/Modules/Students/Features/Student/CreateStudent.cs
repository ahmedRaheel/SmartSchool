using Dapper;
using SmartSchool.Application.Persistence;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Students.Models;
using SmartSchool.Modules.Students.Persistence;
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
		Guid? UserId,
		string FirstName,
		string? LastName,
		DateOnly? DateOfBirth,
		string? Gender,
		byte[]? Photo,
		string? PhotoContentType,
		string? PhotoFileName,
		DateOnly? AdmissionDate,
		string Status) : IRequest<Result<Response>>;

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.SchoolId).NotEmpty();
			RuleFor(x => x.BranchId).NotEmpty();
			RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
		}
	}

	public sealed class Handler(IStudentCommand entityCommand, IDbConnectionFactory connectionFactory)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
		{
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var validScope = await connection.ExecuteScalarAsync<bool>(new CommandDefinition(
                "SELECT EXISTS(SELECT 1 FROM org.campus c WHERE c.tenant_id=@TenantId AND c.school_id=@SchoolId AND c.campus_id=@BranchId)",
                new { TenantId = request.TenantId!.Value, request.SchoolId, request.BranchId }, cancellationToken: cancellationToken));
            if (!validScope) return Result<Response>.Failure(Error.Validation("Selected branch does not belong to the selected school and tenant."));

			var entity = StudentEntity.Create(
				request.TenantId!.Value,
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
				"PENDING_APPROVAL");

			await entityCommand.AddAsync(entity, cancellationToken);
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
