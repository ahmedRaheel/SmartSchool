using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.Modules.Organization.Persistence;
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

	public sealed class Handler(ICampusQuery entityQuery)
		: IRequestHandler<Query, Result<PagedResult<Response>>>
	{
		public async Task<Result<PagedResult<Response>>> HandleAsync(
			Query request,
			CancellationToken cancellationToken)
		{
			var pageRequest = new PageRequest(request.Page, request.PageSize);
			var page = await entityQuery.GetPageAsync(
				request.TenantId,
				pageRequest.NormalizedPage,
				pageRequest.NormalizedPageSize,
				cancellationToken);
			var response = new PagedResult<Response>(
				page.Items.Select(MapResponse).ToArray(),
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

	private static Response MapResponse(CampusEntity entity)
	{
		return new Response(
			entity.TenantId, entity.CampusId, entity.SchoolId, entity.Code, entity.Name, entity.BranchType, entity.BranchGenderTypeId,
			            entity.Address, entity.City, entity.Province, entity.Country, entity.Phone, entity.Fax, entity.Mobile,
			            entity.Email, entity.LogoUrl);
	}
}
