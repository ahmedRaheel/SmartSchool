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

public static class CreateStudentPerformancePrediction
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
		string Code,
		string Name) : IRequest<Result<Response>>;

	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.TenantId).NotEmpty();
			RuleFor(x => x.Code).NotEmpty().MaximumLength(100);
			RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
		}
	}

	public interface ICreateStudentPerformancePrediction
	{
		Task AddAsync(
				StudentPerformancePredictionEntity entity,
				CancellationToken cancellationToken);

		Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken);

	}

	internal sealed class CreateStudentPerformancePredictionDataAccess(
		IApplicationDbContext dbContext) : ICreateStudentPerformancePrediction
	{
		public async Task AddAsync(
				StudentPerformancePredictionEntity entity,
				CancellationToken cancellationToken)
			{
				await dbContext
					.Set<StudentPerformancePredictionEntity>()
					.AddAsync(entity, cancellationToken);
		
				await dbContext.SaveChangesAsync(cancellationToken);
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

	public sealed class Handler(ICreateStudentPerformancePrediction dataAccess)
		: IRequestHandler<Request, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(
			Request request,
			CancellationToken cancellationToken)
		{
			var exists = await dataAccess.ExistsByCodeAsync(
				request.TenantId, request.Code, null, cancellationToken);
			if (exists)
			{
				return Result<Response>.Failure(
					Error.Conflict(
						ErrorMessages.DuplicateCode(nameof(StudentPerformancePredictionEntity), request.Code)));
			}

			var entity = StudentPerformancePredictionEntity.Create(
				request.TenantId,
				request.Code,
				request.Name);

			await dataAccess.AddAsync(entity, cancellationToken);
			return Result<Response>.Success(MapResponse(entity));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPost(
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "student-performance-prediction"),
				async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var result = await mediator.SendAsync<Request, Result<Response>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("CreateStudentPerformancePrediction")
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
