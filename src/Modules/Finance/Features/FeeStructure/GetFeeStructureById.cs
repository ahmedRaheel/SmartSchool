using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Finance.Contracts;
using SmartSchool.Modules.Finance.Models;
using SmartSchool.Modules.Finance.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Finance.Features.FeeStructure;

public static class GetFeeStructureById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<FeeStructureResponse>>;

    public sealed class Handler(IFeeStructureQuery entityQuery)
        : IRequestHandler<Query, Result<FeeStructureResponse>>
    {
        public async Task<Result<FeeStructureResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<FeeStructureResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(FeeStructure))));
            }
            return Result<FeeStructureResponse>.Success(FeeStructureResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "fee-structure"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<FeeStructureResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetFeeStructureById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
