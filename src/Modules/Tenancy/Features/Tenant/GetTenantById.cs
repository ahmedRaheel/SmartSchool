using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Tenancy.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Tenancy.Features.Tenant;

public static class GetTenantById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<Tenant> repository)
    {
        public async Task<Result<Tenant>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<Tenant>.Failure(
                    Error.NotFound("Tenant was not found."));
            }

            return Result<Tenant>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/tenancy/tenant/{id:guid}",
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
            .WithName("GetTenantById")
            .WithTags("Tenancy")
            .RequireAuthorization();

        return endpoints;
    }
}
