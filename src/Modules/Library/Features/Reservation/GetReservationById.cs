using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Library.Contracts;
using SmartSchool.Modules.Library.Models;
using SmartSchool.Modules.Library.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Library.Features.Reservation;

public static class GetReservationById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<ReservationResponse>>;

    public sealed class Handler(IReservationQuery entityQuery)
        : IRequestHandler<Query, Result<ReservationResponse>>
    {
        public async Task<Result<ReservationResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<ReservationResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Reservation))));
            }
            return Result<ReservationResponse>.Success(ReservationResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "reservation"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<ReservationResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetReservationById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
