using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Transport.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Transport.Features.Route;

public static class GetRouteById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<Route> repository)
    {
        public async Task<Result<Route>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<Route>.Failure(
                    Error.NotFound("Route was not found."));
            }

            return Result<Route>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/transport/route/{id:guid}",
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
            .WithName("GetRouteById")
            .WithTags("Transport")
            .RequireAuthorization();

        return endpoints;
    }
}
