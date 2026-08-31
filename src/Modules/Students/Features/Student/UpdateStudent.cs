using SmartSchool.Modules.Students.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Features.Student;

public static class UpdateStudent
{
	public sealed record Response(
		Guid TenantId,
		Guid Id,
		Guid? UserId,	
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
		Guid Id,
		string FirstName,
		string? LastName,
		DateOnly? DateOfBirth,
		string? Gender,
		DateOnly? AdmissionDate,
		string Status) : IRequest<Result<Response>>;

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.TenantId).NotEmpty();
			RuleFor(x => x.Id).NotEmpty();
		}
	}

	public interface IUpdateStudent
	{
		Task UpdateAsync(
				StudentEntity entity,
				CancellationToken cancellationToken);

		Task<StudentEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken);

	}

	internal sealed class UpdateStudentPersistence(
		IStudentsDbContext dbContext) : IUpdateStudent
	{
		public async Task UpdateAsync(
				StudentEntity entity,
				CancellationToken cancellationToken)
			{
				dbContext.Students
					.Update(entity);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}
	
		public Task<StudentEntity?> GetByIdAsync(
			Guid tenantId, Guid id, CancellationToken cancellationToken)
		{
			return dbContext.Students
				.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.StudentId == id, cancellationToken);
		}
}

	public sealed class Handler(IUpdateStudent dataAccess)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
		{
			var entity = await dataAccess.GetByIdAsync(request.TenantId, request.Id, cancellationToken);
			if (entity is null)
			{
				return Result<Response>.Failure(
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(StudentEntity))));
			}

			entity.UpdateDetails(
				request.FirstName,
				request.LastName,
				request.DateOfBirth,
				request.Gender,
				request.AdmissionDate,
				request.Status);

			await dataAccess.UpdateAsync(entity, cancellationToken);
			return Result<Response>.Success(MapResponse(entity));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPut(
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "student"),
				async (Guid id, Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var command = request with { Id = id };
					var result = await mediator.SendAsync<Request, Result<Response>>(command, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("UpdateStudent").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantStudent);
		return endpoints;
	}

	private static Response MapResponse(StudentEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.StudentId,
			entity.UserId,			
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
