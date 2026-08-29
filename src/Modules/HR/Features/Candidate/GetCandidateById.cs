using SmartSchool.Application.Persistence;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Candidate;

public static class GetCandidateById
{
	/// <summary>
	/// Represents the response returned by this CandidateEntity feature.
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

	public interface IGetCandidateById
	{
		Task<Response?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken);

	}

	internal sealed class GetCandidateByIdDataAccess(
		IDbConnectionFactory connectionFactory) : IGetCandidateById
	{
		public async Task<Response?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken)
			{
				const string sql = """
					SELECT
						tenant_id AS "TenantId",
						candidate_id AS "Id",
						code AS "Code",
						name AS "Name",
						metadata_json AS "MetadataJson"
					FROM hr.candidate
					WHERE tenant_id = @TenantId
					  AND candidate_id = @Id
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
						},
						cancellationToken: cancellationToken)).ConfigureAwait(false);
			}
	}

	public sealed class Handler(IGetCandidateById dataAccess)
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
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(CandidateEntity))));
			}
			return Result<Response>.Success(entity);
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapGet(
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "candidate"),
				async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var request = new Query(tenantId, id);
					var result = await mediator.SendAsync<Query, Result<Response>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("GetCandidateById")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}

	private static Response MapResponse(CandidateEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.CandidateId,
			entity.Code,
			entity.Name,
			entity.MetadataJson);
	}
}
