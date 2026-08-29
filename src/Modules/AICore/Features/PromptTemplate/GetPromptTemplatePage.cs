using SmartSchool.Application.Persistence;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;
using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Features.PromptTemplate;

public static class GetPromptTemplatePage
{
	/// <summary>
	/// Represents the response returned by this PromptTemplateEntity feature.
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

	public interface IGetPromptTemplatePage
	{
		Task<PagedResult<Response>> GetPageAsync(
				Guid tenantId,
				int page,
				int pageSize,
				CancellationToken cancellationToken);

	}

	internal sealed class GetPromptTemplatePageDataAccess(
		IDbConnectionFactory connectionFactory) : IGetPromptTemplatePage
	{
		public async Task<PagedResult<Response>> GetPageAsync(
				Guid tenantId,
				int page,
				int pageSize,
				CancellationToken cancellationToken)
			{
				const string countSql = """
					SELECT COUNT(*)
					FROM ai_core.prompt_template
					WHERE tenant_id = @TenantId
					  AND is_active = TRUE;
					""";
		
				const string pageSql = """
					SELECT
					tenant_id AS "TenantId",
					prompt_template_id AS "Id",
					code AS "Code",
					name AS "Name",
					metadata_json AS "MetadataJson"
					FROM ai_core.prompt_template
					WHERE tenant_id = @TenantId
					  AND is_active = TRUE
					ORDER BY prompt_template_id
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

	public sealed class Handler(IGetPromptTemplatePage dataAccess)
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
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "prompt-template"),
				async (Guid tenantId, int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var request = new Query(tenantId, page, pageSize);
					var result = await mediator.SendAsync<Query, Result<PagedResult<Response>>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("GetPromptTemplatePage")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}
}
