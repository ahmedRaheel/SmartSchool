using Microsoft.EntityFrameworkCore;
using Dapper;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Models;
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
		string Status,
        string EnrollmentNumber);

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

	public interface ICreateEnrollment
	{
		Task AddAsync(
				EnrollmentEntity entity,
				CancellationToken cancellationToken);

	}

	internal sealed class CreateEnrollmentDataAccess(
		IApplicationDbContext dbContext,
		IDbConnectionFactory connectionFactory) : ICreateEnrollment
	{
		public async Task AddAsync(
				EnrollmentEntity entity,
				CancellationToken cancellationToken)
			{
				await dbContext
					.Set<EnrollmentEntity>()
					.AddAsync(entity, cancellationToken);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}
	}

	public sealed class Handler(IBusinessNumberGenerator numberGenerator,
		ICreateEnrollment dataAccess)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
		{
			if (await dataAccess.ExistsForAcademicYearAsync(request.TenantId, request.StudentId, request.AcademicYearId, cancellationToken))
			{
				return Result<Response>.Failure(Error.Conflict("The student is already enrolled for this academic year."));
			}

			var student = await dataAccess.GetByIdAsync(request.TenantId, request.StudentId, cancellationToken);
            if (student is null || string.IsNullOrWhiteSpace(student.StudentNumber))
                return Result<Response>.Failure(Error.Validation("Student admission must be approved before enrollment."));
            var enrollmentNumber = await numberGenerator.NextAsync($"ENROLLMENT:{request.StudentId}", $"{student.StudentNumber}-", request.TenantId, 3, cancellationToken);

            var entity = EnrollmentEntity.Create(
				request.TenantId,
				request.StudentId,
                enrollmentNumber,
				request.AcademicYearId,
				request.ClassSectionId,
				request.EnrollmentDate,
				request.Status);

			await dataAccess.AddAsync(entity, cancellationToken);
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
		entity.Status,
        entity.EnrollmentNumber);
}
