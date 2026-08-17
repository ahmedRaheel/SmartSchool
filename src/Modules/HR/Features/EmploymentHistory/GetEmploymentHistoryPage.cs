using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.Modules.HR.Contracts;
using SmartSchool.Modules.HR.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.EmploymentHistory;

public static class GetEmploymentHistoryPage
{
    public sealed record Query(
        Guid TenantId,
        int Page = 1,
        int PageSize = 25) : IRequest<Result<PagedResult<EmploymentHistoryResponse>>>;

    public sealed class Handler(IEmploymentHistoryQuery entityQuery)
        : IRequestHandler<Query, Result<PagedResult<EmploymentHistoryResponse>>>
    {
        public async Task<Result<PagedResult<EmploymentHistoryResponse>>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var pageRequest = new PageRequest(request.Page, request.PageSize);
            var page = await entityQuery.GetPageAsync(
                request.TenantId,
                pageRequest.NormalizedPage,
                pageRequest.NormalizedPageSize,
                cancellationToken);
            var response = new PagedResult<EmploymentHistoryResponse>(
                page.Items.Select(EmploymentHistoryResponse.FromEntity).ToArray(),
                page.Page,
                page.PageSize,
                page.TotalCount);
            return Result<PagedResult<EmploymentHistoryResponse>>.Success(response);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "employment-history"),
                async (Guid tenantId, int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, page, pageSize);
                    var result = await mediator.SendAsync<Query, Result<PagedResult<EmploymentHistoryResponse>>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetEmploymentHistoryPage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
