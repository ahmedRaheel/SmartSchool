using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AICore.Features.KnowledgeCollection;

public static class GetKnowledgeCollectionById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<KnowledgeCollection> repository)
    {
        public async Task<Result<KnowledgeCollection>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<KnowledgeCollection>.Failure(
                    Error.NotFound("KnowledgeCollection was not found."));
            }

            return Result<KnowledgeCollection>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/aicore/knowledge-collection/{id:guid}",
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
            .WithName("GetKnowledgeCollectionById")
            .WithTags("AICore")
            .RequireAuthorization();

        return endpoints;
    }
}
