using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AICore.Features.ModelConfiguration;

public static class GetModelConfigurationById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<ModelConfiguration> repository)
    {
        public async Task<Result<ModelConfiguration>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<ModelConfiguration>.Failure(
                    Error.NotFound("ModelConfiguration was not found."));
            }

            return Result<ModelConfiguration>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/aicore/model-configuration/{id:guid}",
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
            .WithName("GetModelConfigurationById")
            .WithTags("AICore")
            .RequireAuthorization();

        return endpoints;
    }
}
