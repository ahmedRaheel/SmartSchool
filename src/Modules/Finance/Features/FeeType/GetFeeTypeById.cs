using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Finance.Contracts;
using SmartSchool.Modules.Finance.Models;
using SmartSchool.Modules.Finance.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Finance.Features.FeeType;

public static class GetFeeTypeById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<FeeTypeResponse>>;

    public sealed class Handler(IFeeTypeQuery entityQuery)
        : IRequestHandler<Query, Result<FeeTypeResponse>>
    {
        public async Task<Result<FeeTypeResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<FeeTypeResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(FeeType))));
            }
            return Result<FeeTypeResponse>.Success(FeeTypeResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "fee-type"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<FeeTypeResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetFeeTypeById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
