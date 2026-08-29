using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIPrediction.Features.StudentPerformancePrediction;

public static class UpdateStudentPerformancePrediction
{
	/// <summary>
	/// Represents the response returned by this StudentPerformancePredictionEntity feature.
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
		string Code,
		string Name) : IRequest<Result<Response>>;

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.TenantId).NotEmpty();
			RuleFor(x => x.Id).NotEmpty();
			RuleFor(x => x.Code).NotEmpty().MaximumLength(100);
			RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
		}
	}

	public interface IUpdateStudentPerformancePrediction
	{
		Task UpdateAsync(
				StudentPerformancePredictionEntity entity,
				CancellationToken cancellationToken);

		Task<StudentPerformancePredictionEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken);

		Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken);

	}

	internal sealed class UpdateStudentPerformancePredictionDataAccess(
		IApplicationDbContext dbContext) : IUpdateStudentPerformancePrediction
	{
		public async Task UpdateAsync(
				StudentPerformancePredictionEntity entity,
				CancellationToken cancellationToken)
			{
				dbContext
					.Set<StudentPerformancePredictionEntity>()
					.Update(entity);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}
	
		public Task<StudentPerformancePredictionEntity?> GetByIdAsync(
			Guid tenantId, Guid id, CancellationToken cancellationToken)
		{
			return dbContext.Set<StudentPerformancePredictionEntity>()
				.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.StudentPerformancePredictionId == id, cancellationToken);
		}

		public Task<bool> ExistsByCodeAsync(
			Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
		{
			return dbContext.Set<StudentPerformancePredictionEntity>().AnyAsync(
				x => x.TenantId == tenantId
					&& x.Code == code
					&& (!excludingId.HasValue || x.StudentPerformancePredictionId != excludingId.Value),
				cancellationToken);
		}
}

	public sealed class Handler(IUpdateStudentPerformancePrediction dataAccess)
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
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(StudentPerformancePredictionEntity))));
			}

			var exists = await dataAccess.ExistsByCodeAsync(
				request.TenantId, request.Code, request.Id, cancellationToken);
			if (exists)
			{
				return Result<Response>.Failure(
					Error.Conflict(
						ErrorMessages.DuplicateCode(nameof(StudentPerformancePredictionEntity), request.Code)));
			}

			entity.UpdateDetails(
				request.Code,
				request.Name);
			await dataAccess.UpdateAsync(entity, cancellationToken);
			return Result<Response>.Success(MapResponse(entity));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPut(
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "student-performance-prediction"),
				async (Guid id, Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var command = request with { Id = id };
					var result = await mediator.SendAsync<Request, Result<Response>>(
						command, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("UpdateStudentPerformancePrediction")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}

	private static Response MapResponse(StudentPerformancePredictionEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.StudentPerformancePredictionId,
			entity.Code,
			entity.Name,
			entity.MetadataJson);
	}
}
