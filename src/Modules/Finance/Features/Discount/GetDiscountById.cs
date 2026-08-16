using SmartSchool.Modules.Finance;
using SmartSchool.Modules.Finance.Persistence;
using SmartSchool.Modules.Finance.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Finance.Features.Discount;

public static class GetDiscountById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IDiscountQuery query)
    {
        public async Task<Result<Discount>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await query.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<Discount>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Discount))));
            }

            return Result<Discount>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/finance/discount/{id:guid}",
                async (
                    Guid id,
                    Guid tenantId,
                    Handler handler,
                    CancellationToken cancellationToken) =>
                {
                    var query = new Query(tenantId, id);

                    var result = await handler.HandleAsync(
                        query,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("GetDiscountById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
