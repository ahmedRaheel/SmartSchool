using SmartSchool.Application.Persistence;
using SmartSchool.Application.Requests;
using SmartSchool.Modules.Finance.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Finance.Features.Invoice;

public static class GetInvoicePage
{
    public sealed record Query(
        Guid TenantId,
        int Page = 1,
        int PageSize = 25);

    public sealed class Handler(
        IRepository<Invoice> repository)
    {
        public async Task<Result<PagedResult<Invoice>>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var pageRequest = new PageRequest(
                query.Page,
                query.PageSize);

            var result = await repository.GetPageAsync(
                query.TenantId,
                pageRequest.NormalizedPage,
                pageRequest.NormalizedPageSize,
                cancellationToken);

            return Result<PagedResult<Invoice>>.Success(result);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/finance/invoice",
                async (
                    Guid tenantId,
                    int page,
                    int pageSize,
                    Handler handler,
                    CancellationToken cancellationToken) =>
                {
                    var query = new Query(
                        tenantId,
                        page,
                        pageSize);

                    var result = await handler.HandleAsync(
                        query,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("GetInvoicePage")
            .WithTags("Finance")
            .RequireAuthorization();

        return endpoints;
    }
}
