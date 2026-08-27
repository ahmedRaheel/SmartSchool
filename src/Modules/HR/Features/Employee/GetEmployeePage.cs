using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.Modules.HR.Models;
using SmartSchool.Modules.HR.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Employee;

public static class GetEmployeePage
{
	public sealed record Response(
		Guid TenantId,
		Guid Id,
		string? EmployeeNumber,
		string FirstName,
		string? LastName,
		string? CnicNumber,
		string? Email,
		string? Phone,
		DateOnly HireDate,
		string EmploymentTypeCode,
		string Status);

	public sealed record Query(Guid TenantId, int Page = 1, int PageSize = 25)
		: IRequest<Result<PagedResult<Response>>>;

	public sealed class Handler(IEmployeeQuery entityQuery)
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
				ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "employee"),
				async (Guid tenantId, int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
				{
					var result = await mediator.SendAsync<Query, Result<PagedResult<Response>>>(
						new Query(tenantId, page, pageSize), cancellationToken);
					return result.ToHttpResult();
				})
			.WithName("GetEmployeePage").WithTags(ModuleConstants.Name).RequireAuthorization();
		return endpoints;
	}

	private static Response MapResponse(EmployeeEntity entity)
	{
		return new Response(
			entity.TenantId,
			entity.EmployeeId,
			entity.EmployeeNumber,
			entity.FirstName,
			entity.LastName,
			entity.CnicNumber,
			entity.Email,
			entity.Phone,
			entity.HireDate,
			entity.EmploymentTypeCode,
			entity.Status);
	}
}
