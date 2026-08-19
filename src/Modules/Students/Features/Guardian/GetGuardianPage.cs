using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.Modules.Students.Models;
using SmartSchool.Modules.Students.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Features.Guardian;

public static class GetGuardianPage
{
	public sealed record Response(
		Guid TenantId,
		Guid Id,
		Guid? UserId,
		string FullName,
		string? CnicNumber,
		string? Email,
		string? Phone);

	public sealed record Query(Guid TenantId, int Page = 1, int PageSize = 25)
		: IRequest<Result<PagedResult<Response>>>;

	public sealed class Handler(IGuardianQuery entityQuery)
		: IRequestHandler<Query, Result<PagedResult<Response>>>
	{
		public async Task<Result<PagedResult<Response>>> HandleAsync(Query request, CancellationToken cancellationToken)
		{
			var pageRequest = new PageRequest(request.Page, request.PageSize);
			var page = await entityQuery.GetPageAsync(
				request.TenantId, pageRequest.NormalizedPage, pageRequest.NormalizedPageSize, cancellationToken);
			var response = new PagedResult<Response>(
				page.Items.Select(MapResponse).ToArray(), page.Page, page.PageSize, page.TotalCount);
			return Result<PagedResult<Response>>.Success(response);
		}
	}

	public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapGet(
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "guardian"),
				async (Guid tenantId, int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var result = await mediator.SendAsync<Query, Result<PagedResult<Response>>>(
						new Query(tenantId, page, pageSize), cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("GetGuardianPage").WithTags(ModuleConstants.Name).RequireAuthorization();
		return endpoints;
	}

	private static Response MapResponse(GuardianEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.Id,
			entity.UserId,
			entity.FullName,
			entity.CnicNumber,
			entity.Email,
			entity.Phone);
	}
}
