using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.Modules.Library.Contracts;
using SmartSchool.Modules.Library.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Library.Features.Reservation;

public static class GetReservationPage
{
    public sealed record Query(
        Guid TenantId,
        int Page = 1,
        int PageSize = 25) : IRequest<Result<PagedResult<ReservationResponse>>>;

    public sealed class Handler(IReservationQuery entityQuery)
        : IRequestHandler<Query, Result<PagedResult<ReservationResponse>>>
    {
        public async Task<Result<PagedResult<ReservationResponse>>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var pageRequest = new PageRequest(request.Page, request.PageSize);
            var page = await entityQuery.GetPageAsync(
                request.TenantId,
                pageRequest.NormalizedPage,
                pageRequest.NormalizedPageSize,
                cancellationToken);
            var response = new PagedResult<ReservationResponse>(
                page.Items.Select(ReservationResponse.FromEntity).ToArray(),
                page.Page,
                page.PageSize,
                page.TotalCount);
            return Result<PagedResult<ReservationResponse>>.Success(response);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "reservation"),
                async (Guid tenantId, int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, page, pageSize);
                    var result = await mediator.SendAsync<Query, Result<PagedResult<ReservationResponse>>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetReservationPage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
