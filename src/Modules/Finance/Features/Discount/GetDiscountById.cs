using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Finance.Contracts;
using SmartSchool.Modules.Finance.Models;
using SmartSchool.Modules.Finance.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Finance.Features.Discount;

public static class GetDiscountById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<DiscountResponse>>;

    public sealed class Handler(IDiscountQuery entityQuery)
        : IRequestHandler<Query, Result<DiscountResponse>>
    {
        public async Task<Result<DiscountResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<DiscountResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Discount))));
            }
            return Result<DiscountResponse>.Success(DiscountResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "discount"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<DiscountResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetDiscountById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
