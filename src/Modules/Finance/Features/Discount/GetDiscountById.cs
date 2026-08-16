using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Finance.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Finance.Features.Discount;

public static class GetDiscountById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<Discount> repository)
    {
        public async Task<Result<Discount>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<Discount>.Failure(
                    Error.NotFound("Discount was not found."));
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
            .WithTags("Finance")
            .RequireAuthorization();

        return endpoints;
    }
}
