using SmartSchool.Modules.Documents;
using SmartSchool.Modules.Documents.Persistence;
using SmartSchool.Modules.Documents.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Documents.Features.GeneratedDocument;

public static class GetGeneratedDocumentById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IGeneratedDocumentQuery query)
    {
        public async Task<Result<GeneratedDocument>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await query.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<GeneratedDocument>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(GeneratedDocument))));
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
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
