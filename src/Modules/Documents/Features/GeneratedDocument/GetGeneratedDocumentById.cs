using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Documents.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Features.GeneratedDocument;

public static class GetGeneratedDocumentById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<GeneratedDocument> repository)
    {
        public async Task<Result<GeneratedDocument>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<GeneratedDocument>.Failure(
                    Error.NotFound("GeneratedDocument was not found."));
            }

            return Result<GeneratedDocument>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/documents/generated-document/{id:guid}",
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
            .WithName("GetGeneratedDocumentById")
            .WithTags("Documents")
            .RequireAuthorization();

        return endpoints;
    }
}
