using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.CourseSelection;

public static class UpdateCourseSelection
{
	/// <summary>
	/// Represents the response returned by this CourseSelectionEntity feature.
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
		Guid Id,
		string Name) : IRequest<Result<Response>>;

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.TenantId).NotEmpty();
			RuleFor(x => x.Id).NotEmpty();
			RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
		}
	}

	public interface IUpdateCourseSelection
	{
		Task UpdateAsync(
				CourseSelectionEntity entity,
				CancellationToken cancellationToken);
Task<CourseSelectionEntity?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken);

	}

	internal sealed class UpdateCourseSelectionPersistence(IApplicationDbContext dbContext) : IUpdateCourseSelection
	{
		public async Task UpdateAsync(
				CourseSelectionEntity entity,
				CancellationToken cancellationToken)
			{
				dbContext
					.Set<CourseSelectionEntity>()
					.Update(entity);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}

		public async Task<CourseSelectionEntity?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken)
			{
				return await dbContext
					.Set<CourseSelectionEntity>()
					.FirstOrDefaultAsync(
						x => x.TenantId == tenantId
							&& x.CourseOfferingId == id,
						cancellationToken);
			}
	}

	public sealed class Handler(IUpdateCourseSelection dataAccess)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(
			Request request,
			CancellationToken cancellationToken)
		{
			var entity = await dataAccess.GetByIdAsync(
				request.TenantId, request.Id, cancellationToken);
			if (entity is null)
			{
				return Result<Response>.Failure(
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(CourseSelectionEntity))));
			}


			entity.UpdateDetails(
				entity.Code,
				request.Name);
			await dataAccess.UpdateAsync(entity, cancellationToken);
			return Result<Response>.Success(MapResponse(entity));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPut(
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "course-selection"),
				async (Guid id, Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var command = request with { Id = id };
					var result = await mediator.SendAsync<Request, Result<Response>>(
						command, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("UpdateCourseSelection")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantTeacher);
		return endpoints;
	}

	private static Response MapResponse(CourseSelectionEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.StudentCourseEnrollmentId,
			entity.Code,
			entity.Name,
			entity.MetadataJson);
	}
}
