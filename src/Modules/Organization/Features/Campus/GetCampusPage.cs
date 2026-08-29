using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;
using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Features.Campus;

public static class GetCampusPage
{
	/// <summary>
	/// Represents the response returned by this CampusEntity feature.
	/// </summary>
	/// <param name="TenantId">The owning tenant identifier.</param>
	/// <param name="Id">The entity identifier.</param>
	/// <param name="Code">The business code.</param>
	/// <param name="Name">The display name.</param>
	public sealed record Response(
		Guid TenantId, Guid Id, Guid SchoolId, string Code, string Name, string BranchType, Guid BranchGenderTypeId,
		        string? Address, string? City, string? Province, string? Country, string? Phone, string? Fax,
		        string? Mobile, string? Email, string? LogoUrl);

	public sealed record Query(
		Guid? TenantId,
		int Page = 1,
		int PageSize = 25) : IRequest<Result<PagedResult<Response>>>;

	public interface IGetCampusPage
	{
		Task<PagedResult<Response>> GetPageAsync(
				Guid? tenantId,
				int page,
				int pageSize,
				CancellationToken cancellationToken);

	}

	internal sealed class GetCampusPageDataAccess(
		IApplicationDbContext dbContext,
		IDbConnectionFactory connectionFactory) : IGetCampusPage
	{
		public async Task<PagedResult<Response>> GetPageAsync(
				Guid? tenantId,
				int page,
				int pageSize,
				CancellationToken cancellationToken)
			{
				const string countSql = """
					SELECT COUNT(*)
					FROM org.campus
					WHERE (@TenantId IS NULL OR tenant_id = @TenantId)
					  AND is_active = TRUE;
					""";
		
				const string pageSql = """
					SELECT
					tenant_id AS "TenantId",
					id AS "Id",
					school_id AS "SchoolId",
					code AS "Code",
					name AS "Name",
					branch_type AS "BranchType",
					branch_gender_type_id AS "BranchGenderTypeId",
					address AS "Address",
					city AS "City",
					province AS "Province",
					country AS "Country",
					phone AS "Phone",
					fax AS "Fax",
					mobile AS "Mobile",
					email AS "Email",
					logo_url AS "LogoUrl"
					FROM org.campus
					WHERE (@TenantId IS NULL OR tenant_id = @TenantId)
					  AND is_active = TRUE
					ORDER BY campus_id
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
						cancellationToken: cancellationToken));
		
				var items = (await connection.QueryAsync<Response>(
					new CommandDefinition(
						pageSql,
						parameters,
						cancellationToken: cancellationToken)))
					.AsList();
		
				return new PagedResult<Response>(
					items,
					page,
					pageSize,
					totalCount);
			}
	}

	public sealed class Handler(IGetCampusPage dataAccess)
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
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "campus"),
				async (Guid? tenantId, int page, int pageSize, SmartSchool.Application.Identity.ITenantScope tenantScope, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var effectiveTenantId = tenantScope.Resolve(tenantId);
                    var request = new Query(effectiveTenantId, page, pageSize);
					var result = await mediator.SendAsync<Query, Result<PagedResult<Response>>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("GetCampusPage")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantAdmin);
		return endpoints;
	}
}
