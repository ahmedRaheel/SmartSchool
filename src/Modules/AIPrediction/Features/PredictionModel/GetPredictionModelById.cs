using SmartSchool.Application.Persistence;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIPrediction.Features.PredictionModel;

public static class GetPredictionModelById
{
	/// <summary>
	/// Represents the response returned by this PredictionModelEntity feature.
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

	public interface IGetPredictionModelById
	{
		Task<Response?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken);

	}

	internal sealed class GetPredictionModelByIdPersistence(
		IDbConnectionFactory connectionFactory) : IGetPredictionModelById
	{
		public async Task<Response?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken)
		{
			const string sql = """
					SELECT
						tenant_id AS "TenantId",
						prediction_model_id AS "Id",
						code AS "Code",
						name AS "Name",
						metadata_json AS "MetadataJson"
					FROM ai.prediction_model
					WHERE tenant_id = @TenantId
					  AND prediction_model_id = @Id
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

	public sealed class Handler(IGetPredictionModelById dataAccess)
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
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(PredictionModelEntity))));
			}
			return Result<Response>.Success(entity);
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapGet(
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "prediction-model"),
				async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var request = new Query(tenantId, id);
					var result = await mediator.SendAsync<Query, Result<Response>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("GetPredictionModelById")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}
}
