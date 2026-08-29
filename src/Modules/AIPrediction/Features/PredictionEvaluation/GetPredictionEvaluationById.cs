using SmartSchool.Application.Persistence;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIPrediction.Features.PredictionEvaluation;

public static class GetPredictionEvaluationById
{
	/// <summary>
	/// Represents the response returned by this PredictionEvaluationEntity feature.
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

	public sealed record Query(
		Guid TenantId,
		Guid Id) : IRequest<Result<Response>>;

	public interface IGetPredictionEvaluationById
	{
		Task<Response?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken);

	}

	internal sealed class GetPredictionEvaluationByIdPersistence(
		IDbConnectionFactory connectionFactory) : IGetPredictionEvaluationById
	{
		public async Task<Response?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken)
		{
			const string sql = """
					SELECT
						tenant_id AS "TenantId",
						prediction_evaluation_id AS "Id",
						code AS "Code",
						name AS "Name",
						metadata_json AS "MetadataJson"
					FROM ai.prediction_evaluation
					WHERE tenant_id = @TenantId
					  AND prediction_evaluation_id = @Id
					  AND is_active = TRUE;
					""";

			await using var connection =
				await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

			return await connection.QuerySingleOrDefaultAsync<Response>(
				new CommandDefinition(
					sql,
					new
					{
						TenantId = tenantId,
						Id = id
					}));
		}
	}

	public sealed class Handler(IGetPredictionEvaluationById dataAccess)
		: IRequestHandler<Query, Result<Response>>
	{
		public async Task<Result<Response>> HandleAsync(
			Query request,
			CancellationToken cancellationToken)
		{
			var entity = await dataAccess.GetByIdAsync(
				request.TenantId, request.Id, cancellationToken);
			if (entity is null)
			{
				return Result<Response>.Failure(
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(PredictionEvaluationEntity))));
			}
			return Result<Response>.Success(entity);
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapGet(
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "prediction-evaluation"),
				async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var request = new Query(tenantId, id);
					var result = await mediator.SendAsync<Query, Result<Response>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("GetPredictionEvaluationById")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}
}
