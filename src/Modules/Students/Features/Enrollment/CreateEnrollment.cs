using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Students.Models;
using SmartSchool.Modules.Students.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Features.Enrollment;

public static class CreateEnrollment
{
	public sealed record Response(
		Guid TenantId,
		Guid Id,
		Guid StudentId,
		Guid AcademicYearId,
		Guid ClassSectionId,
		DateOnly EnrollmentDate,
		string Status);

	public sealed record Request(
		Guid TenantId,
		Guid StudentId,
		Guid AcademicYearId,
		Guid ClassSectionId,
		DateOnly EnrollmentDate,
		string Status) : IRequest<Result<Response>>;

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.TenantId).NotEmpty();
			RuleFor(x => x.StudentId).NotEmpty();
			RuleFor(x => x.AcademicYearId).NotEmpty();
			RuleFor(x => x.ClassSectionId).NotEmpty();
			RuleFor(x => x.EnrollmentDate).NotEmpty();
			RuleFor(x => x.Status).NotEmpty().MaximumLength(30);
		}
	}

	public sealed class Handler(IEnrollmentQuery query, IEnrollmentCommand command)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
		{
			if (await query.ExistsForAcademicYearAsync(request.TenantId, request.StudentId, request.AcademicYearId, cancellationToken))
			{
				return Result<Response>.Failure(Error.Conflict("The student is already enrolled for this academic year."));
			}

			var entity = EnrollmentEntity.Create(
				request.TenantId,
				request.StudentId,
				request.AcademicYearId,
				request.ClassSectionId,
				request.EnrollmentDate,
				request.Status);

			await command.AddAsync(entity, cancellationToken);
			return Result<Response>.Success(Map(entity));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPost(
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "enrollment"),
				async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var result = await mediator.SendAsync<Request, Result<Response>>(request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("CreateEnrollment")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantStudent);
		return endpoints;
	}

	private static Response Map(EnrollmentEntity entity) => new(
		entity.TenantId,
		entity.StudentEnrollmentId,
		entity.StudentId,
		entity.AcademicYearId,
		entity.ClassSectionId,
		entity.EnrollmentDate,
		entity.Status);
}
