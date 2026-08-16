using SmartSchool.Modules.Tenancy;
using SmartSchool.Modules.Tenancy.Persistence;
using SmartSchool.Application.Requests;
using SmartSchool.Modules.Tenancy.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Tenancy.Features.Subscription;

public static class GetSubscriptionPage
{
    public sealed record Query(
        Guid TenantId,
        int Page = 1,
        int PageSize = 25);

    public sealed class Handler(
        ISubscriptionQuery query)
    {
        public async Task<Result<PagedResult<Subscription>>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var pageRequest = new PageRequest(
                query.Page,
                query.PageSize);

            var result = await query.GetPageAsync(
                query.TenantId,
                pageRequest.NormalizedPage,
                pageRequest.NormalizedPageSize,
                cancellationToken);

            return Result<PagedResult<Subscription>>.Success(result);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "subscription"),
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
            .WithName("GetSubscriptionPage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
