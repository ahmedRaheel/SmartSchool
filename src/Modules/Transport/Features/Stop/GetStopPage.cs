using SmartSchool.Modules.Transport;
using SmartSchool.Modules.Transport.Persistence;
using SmartSchool.Application.Requests;
using SmartSchool.Modules.Transport.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Transport.Features.Stop;

public static class GetStopPage
{
    public sealed record Query(
        Guid TenantId,
        int Page = 1,
        int PageSize = 25);

    public sealed class Handler(
        IStopQuery query)
    {
        public async Task<Result<PagedResult<Stop>>> HandleAsync(
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

            return Result<PagedResult<Stop>>.Success(result);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "stop"),
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
            .WithName("GetStopPage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
