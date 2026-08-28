using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.Modules.Organization.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.Department;

public static class GetDepartmentPage
{
	/// <summary>
	/// Represents the response returned by this DepartmentEntity feature.
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
	Guid? CampusId,
	Guid? HeadOfDepartmentEmployeeId,
	string? MetadataJson);

	public sealed record Query(
		Guid TenantId,
		Guid? BranchId = null,
		int Page = 1,
		int PageSize = 25) : IRequest<Result<PagedResult<Response>>>;

	public sealed class Handler(IDepartmentQuery entityQuery)
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
			var pageItems = request.BranchId.HasValue
				? page.Items.Where(x => x.CampusId == request.BranchId.Value)
				: page.Items;
			var response = new PagedResult<Response>(
				pageItems.Select(MapResponse).ToArray(),
				page.Page,
				page.PageSize,
				page.TotalCount);
			return Result<PagedResult<Response>>.Success(response);
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapGet(
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "department"),
				async (Guid tenantId, Guid? branchId, int? page, int? pageSize, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var request = new Query(tenantId, branchId, page ?? 1, pageSize ?? 25);
					var result = await mediator.SendAsync<Query, Result<PagedResult<Response>>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("GetDepartmentPage")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantAdmin);
		return endpoints;
	}

	private static Response MapResponse(
		SmartSchool.Modules.Organization.Models.DepartmentEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.DepartmentId,
			entity.Code,
			entity.Name,
			entity.CampusId,
			entity.HeadOfDepartmentEmployeeId,
			entity.MetadataJson);
	}
}
