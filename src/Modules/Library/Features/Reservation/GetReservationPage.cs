using SmartSchool.Application.Persistence;
using SmartSchool.Application.Requests;
using SmartSchool.Modules.Library.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Library.Features.Reservation;

public static class GetReservationPage
{
    public sealed record Query(
        Guid TenantId,
        int Page = 1,
        int PageSize = 25);

    public sealed class Handler(
        IRepository<Reservation> repository)
    {
        public async Task<Result<PagedResult<Reservation>>> HandleAsync(
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

            return Result<PagedResult<Reservation>>.Success(result);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/library/reservation",
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
            .WithName("GetReservationPage")
            .WithTags("Library")
            .RequireAuthorization();

        return endpoints;
    }
}
