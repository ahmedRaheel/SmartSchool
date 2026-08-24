using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.Modules.Library.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Library.Features.Book;

public static class GetBookPage
{
	/// <summary>
	/// Represents the response returned by this BookEntity feature.
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

	public sealed class Handler(IBookQuery entityQuery)
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
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "book"),
				async (Guid tenantId, int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var request = new Query(tenantId, page, pageSize);
					var result = await mediator.SendAsync<Query, Result<PagedResult<Response>>>(
						request, cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("GetBookPage")
			.WithTags(ModuleConstants.Name)
			.RequireAuthorization();
		return endpoints;
	}

	private static Response MapResponse(
		SmartSchool.Modules.Library.Models.BookEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.BookId,
			entity.Code,
			entity.Name,
			entity.MetadataJson);
	}
}
