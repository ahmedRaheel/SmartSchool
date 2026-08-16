using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AICore.Features.KnowledgeDocument;

public static class GetKnowledgeDocumentById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<KnowledgeDocument> repository)
    {
        public async Task<Result<KnowledgeDocument>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<KnowledgeDocument>.Failure(
                    Error.NotFound("KnowledgeDocument was not found."));
            }

            return Result<KnowledgeDocument>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/aicore/knowledge-document/{id:guid}",
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
            .WithName("GetKnowledgeDocumentById")
            .WithTags("AICore")
            .RequireAuthorization();

        return endpoints;
    }
}
