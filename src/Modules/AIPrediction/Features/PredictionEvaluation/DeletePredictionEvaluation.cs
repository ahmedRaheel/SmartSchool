using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIPrediction.Features.PredictionEvaluation;

public static class DeletePredictionEvaluation
{
	public sealed record Command(
		Guid TenantId,
		Guid Id) : IRequest<Result<Response>>;

	public sealed record Response(
		Guid TenantId,
		Guid Id);

	public interface IDeletePredictionEvaluation
	{
		Task DeleteAsync(
				PredictionEvaluationEntity entity,
				CancellationToken cancellationToken);

		Task<PredictionEvaluationEntity?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken);

	}

	internal sealed class DeletePredictionEvaluationDataAccess(
		IApplicationDbContext dbContext,
		IDbConnectionFactory connectionFactory) : IDeletePredictionEvaluation
	{
		public async Task DeleteAsync(
				PredictionEvaluationEntity entity,
				CancellationToken cancellationToken)
			{
				dbContext
					.Set<PredictionEvaluationEntity>()
					.Remove(entity);
		
				await dbContext.SaveChangesAsync(cancellationToken);
			}

		public async Task<PredictionEvaluationEntity?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken)
			{
				const string sql = """
					SELECT *
					FROM ai.prediction_evaluation
					WHERE tenant_id = @TenantId
					  AND prediction_evaluation_id = @Id
					  AND is_active = TRUE;
					""";
		
				await using var connection =
					await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
		
				return await connection.QuerySingleOrDefaultAsync<PredictionEvaluationEntity>(
					new CommandDefinition(
						sql,
						new
						{
							TenantId = tenantId,
							Id = id
						},
						cancellationToken: cancellationToken)).ConfigureAwait(false);
			}
	}

	public sealed class Handler(IDeletePredictionEvaluation dataAccess)
		: IRequestHandler<Command, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(
			Command request,
			CancellationToken cancellationToken)
		{
			var entity = await dataAccess.GetByIdAsync(
				request.TenantId, request.Id, cancellationToken);
			if (entity is null)
			{
				return Result<Response>.Failure(
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(PredictionEvaluationEntity))));
			}
			await dataAccess.DeleteAsync(entity, cancellationToken);
			return Result<Response>.Success(new Response(request.TenantId, request.Id));
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapDelete(
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "prediction-evaluation"),
				async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var request = new Command(tenantId, id);
					var result = await mediator.SendAsync<Command, Result<Response>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("DeletePredictionEvaluation")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}
}
