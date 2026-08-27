using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.Modules.Organization.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.School;

public static class GetSchoolPage
{
	/// <summary>
	/// Represents the response returned by this SchoolEntity feature.
	/// </summary>
	/// <param name="TenantId">The owning tenant identifier.</param>
	/// <param name="Id">The entity identifier.</param>
	/// <param name="Code">The business code.</param>
	/// <param name="Name">The display name.</param>
	public sealed record Response(
		Guid TenantId, Guid Id, string Code, string Name, string? RegistrationNumber,
		        string? Email, string? Phone, string? Fax, string? Website, string? Address,
		        string? City, string? Province, string? Country, string? LogoUrl);

	public sealed record Query(
		Guid? TenantId,
		int Page = 1,
		int PageSize = 25) : IRequest<Result<PagedResult<Response>>>;

	public sealed class Handler(ISchoolQuery entityQuery)
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
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "school"),
				async (Guid? tenantId, int page, int pageSize, SmartSchool.Application.Identity.ITenantScope tenantScope, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var effectiveTenantId = tenantScope.Resolve(tenantId);
                    var request = new Query(effectiveTenantId, page, pageSize);
					var result = await mediator.SendAsync<Query, Result<PagedResult<Response>>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("GetSchoolPage")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantAdmin);
		return endpoints;
	}

	private static Response MapResponse(
		SmartSchool.Modules.Organization.Models.SchoolEntity entity)
	{
		return new Response(
			entity.TenantId, entity.SchoolId, entity.Code, entity.Name, entity.RegistrationNumber,
			            entity.Email, entity.Phone, entity.Fax, entity.Website, entity.Address, entity.City,
			            entity.Province, entity.Country, entity.LogoUrl);
	}
}
