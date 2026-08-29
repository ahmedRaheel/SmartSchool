using SmartSchool.Application.Persistence;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Examinations.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Examinations.Features.Exam;

public static class GetExamById
{
	/// <summary>
	/// Represents the response returned by this ExamEntity feature.
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

	public interface IGetExamById
	{
		Task<Response?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken);

	}

	internal sealed class GetExamByIdDataAccess(
		IDbConnectionFactory connectionFactory) : IGetExamById
	{
		public async Task<Response?> GetByIdAsync(
				Guid tenantId,
				Guid id,
				CancellationToken cancellationToken)
			{
				const string sql = """
					SELECT
						tenant_id AS "TenantId",
						exam_id AS "Id",
						code AS "Code",
						name AS "Name",
						metadata_json AS "MetadataJson"
					FROM exam.exam
					WHERE tenant_id = @TenantId
					  AND exam_id = @Id
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

	public sealed class Handler(IGetExamById dataAccess)
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
					Error.NotFound(ErrorMessages.EntityNotFound(nameof(ExamEntity))));
			}
			return Result<Response>.Success(entity);
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapGet(
				ApiRoutes.EntityById(ModuleConstants.RouteSegment, "exam"),
				async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var request = new Query(tenantId, id);
					var result = await mediator.SendAsync<Query, Result<Response>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("GetExamById")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}

	private static Response MapResponse(ExamEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.ExamId,
			entity.Code,
			entity.Name,
			entity.MetadataJson);
	}
}
