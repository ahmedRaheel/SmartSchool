using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Students.Models;
using SmartSchool.Modules.Students.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Features.Student;

public static class CreateStudent
{
	public sealed record Response(
		Guid TenantId,
		Guid Id,
		Guid? UserId,
		string StudentNumber,
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
		Guid TenantId,
		Guid? UserId,
		string StudentNumber,
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
			RuleFor(x => x.TenantId).NotEmpty();
			RuleFor(x => x.StudentNumber).NotEmpty();
			RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
		}
	}

	public sealed class Handler(IStudentQuery entityQuery, IStudentCommand entityCommand)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
		{
			var exists = await entityQuery.ExistsByStudentNumberAsync(
				request.TenantId, request.StudentNumber, null, cancellationToken);
			if (exists)
			{
				return Result<Response>.Failure(
					Error.Conflict("Student with the supplied StudentNumber already exists."));
			}

			var entity = StudentEntity.Create(
				request.TenantId,
				request.UserId,
				request.StudentNumber,
				request.FirstName,
				request.LastName,
				request.DateOfBirth,
				request.Gender,
				request.Photo,
				request.PhotoContentType,
				request.PhotoFileName,
				request.AdmissionDate,
				request.Status);

			await entityCommand.AddAsync(entity, cancellationToken);
			return Result<Response>.Success(MapResponse(entity));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPost(
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "student"),
				async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
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
			entity.Id,
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
