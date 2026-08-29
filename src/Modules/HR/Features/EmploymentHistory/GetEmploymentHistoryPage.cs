using SmartSchool.Application.Persistence;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Features.EmploymentHistory;

public static class GetEmploymentHistoryPage
{
	/// <summary>
	/// Represents the response returned by this EmploymentHistoryEntity feature.
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
		int Page = 1,
		int PageSize = 25) : IRequest<Result<PagedResult<Response>>>;

	public interface IGetEmploymentHistoryPage
	{
		Task<PagedResult<Response>> GetPageAsync(
				Guid tenantId,
				int page,
				int pageSize,
				CancellationToken cancellationToken);

	}

	internal sealed class GetEmploymentHistoryPageDataAccess(
		IDbConnectionFactory connectionFactory) : IGetEmploymentHistoryPage
	{
		public async Task<PagedResult<Response>> GetPageAsync(
				Guid tenantId,
				int page,
				int pageSize,
				CancellationToken cancellationToken)
			{
				const string countSql = """
					SELECT COUNT(*)
					FROM public.EmploymentHistory
					WHERE tenant_id = @TenantId
					  AND is_active = TRUE;
					""";
		
				const string pageSql = """
					SELECT
					tenant_id AS "TenantId",
					employmenthistory_id AS "Id",
					code AS "Code",
					name AS "Name",
					metadata_json AS "MetadataJson"
					FROM public.EmploymentHistory
					WHERE tenant_id = @TenantId
					  AND is_active = TRUE
					ORDER BY employmenthistory_id
					LIMIT @PageSize OFFSET @Offset;
					""";
		
				await using var connection =
					await connectionFactory.OpenConnectionAsync(cancellationToken);
		
				var parameters = new
				{
					TenantId = tenantId,
					PageSize = pageSize,
					Offset = (page - 1) * pageSize
				};
		
				var totalCount = await connection.ExecuteScalarAsync<long>(
					new CommandDefinition(
						countSql,
						parameters,
						cancellationToken: cancellationToken)).ConfigureAwait(false);
		
				var items = (await connection.QueryAsync<Response>(
					new CommandDefinition(
						pageSql,
						parameters,
						cancellationToken: cancellationToken)).ConfigureAwait(false))
					.AsList();
		
				return new PagedResult<Response>(
					items,
					page,
					pageSize,
					totalCount);
			}
	}

	public sealed class Handler(IGetEmploymentHistoryPage dataAccess)
		: IRequestHandler<Query, Result<PagedResult<Response>>>
	{
		public async Task<Result<PagedResult<Response>>> HandleAsync(
			Query request,
			CancellationToken cancellationToken)
		{
			var pageRequest = new PageRequest(request.Page, request.PageSize);
			var page = await dataAccess.GetPageAsync(
				request.TenantId,
				pageRequest.NormalizedPage,
				pageRequest.NormalizedPageSize,
				cancellationToken);
			var response = new PagedResult<Response>(
				page.Items,
				page.Page,
				page.PageSize,
				page.TotalCount);
			return Result<PagedResult<Response>>.Success(response);
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapGet(
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "employment-history"),
				async (Guid tenantId, int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var request = new Query(tenantId, page, pageSize);
					var result = await mediator.SendAsync<Query, Result<PagedResult<Response>>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("GetEmploymentHistoryPage")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}

	private static Response MapResponse(EmploymentHistoryEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.EmploymentHistoryId,
			entity.Code,
			entity.Name,
			entity.MetadataJson);
	}
}
