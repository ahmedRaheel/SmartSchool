using SmartSchool.Application.Persistence;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Audit.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Audit.Features.AuditLog;

public static class GetAuditLogById
{
	/// <summary>
	/// Represents the response returned by this AuditLogEntity feature.
	/// </summary>
	/// <param name="TenantId">The owning tenant identifier.</param>
	/// <param name="Id">The entity identifier.</param>
	/// <param name="Code">The business code.</param>
	/// <param name="Name">The display name.</param>
	public sealed record Response(
	Guid TenantId,
	long Id,
	string Code,
	string Name,
	string? MetadataJson);

	private sealed record Row(
		Guid TenantId,
		long Id,
		string Code,
		string Name,
		string? MetadataJson);

	public sealed record Query(
		Guid TenantId,
		long Id) : IRequest<Result<Response>>;

	public interface IGetAuditLogById
	{
		Task<Response?> GetByIdAsync(
				Guid tenantId,
				long id,
				CancellationToken cancellationToken);

	}

	internal sealed class GetAuditLogByIdPersistence(
		IDbConnectionFactory connectionFactory) : IGetAuditLogById
	{
		public async Task<Response?> GetByIdAsync(
				Guid tenantId,
				long id,
				CancellationToken cancellationToken)
			{
				const string sql = """
					SELECT
						tenant_id AS "TenantId",
						audit_log_id AS "Id",
						code AS "Code",
						name AS "Name",
						metadata_json::text AS "MetadataJson"
					FROM audit.audit_log
					WHERE tenant_id = @TenantId
					  AND audit_log_id = @Id
					  AND is_active = TRUE;
					""";
		
				await using var connection =
					await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
		
				var row = await connection.QuerySingleOrDefaultAsync<Row>(
					new CommandDefinition(
						sql,
						new { TenantId = tenantId, Id = id },
						cancellationToken: cancellationToken)).ConfigureAwait(false);

				return row is null
					? null
					: new Response(row.TenantId, row.Id, row.Code, row.Name, row.MetadataJson);
			}
	}

	public sealed class Handler(IGetAuditLogById dataAccess)
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
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(AuditLogEntity))));
			}
			return Result<Response>.Success(entity);
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapGet(
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "audit-log"),
				async (long id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var request = new Query(tenantId, id);
					var result = await mediator.SendAsync<Query, Result<Response>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("GetAuditLogById")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}
}
