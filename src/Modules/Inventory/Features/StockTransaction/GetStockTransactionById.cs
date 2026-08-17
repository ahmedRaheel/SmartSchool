using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Inventory.Contracts;
using SmartSchool.Modules.Inventory.Models;
using SmartSchool.Modules.Inventory.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Inventory.Features.StockTransaction;

public static class GetStockTransactionById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<StockTransactionResponse>>;

    public sealed class Handler(IStockTransactionQuery entityQuery)
        : IRequestHandler<Query, Result<StockTransactionResponse>>
    {
        public async Task<Result<StockTransactionResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<StockTransactionResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(StockTransaction))));
            }
            return Result<StockTransactionResponse>.Success(StockTransactionResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "stock-transaction"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<StockTransactionResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetStockTransactionById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
