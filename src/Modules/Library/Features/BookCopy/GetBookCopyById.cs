using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Library.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Library.Features.BookCopy;

public static class GetBookCopyById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<BookCopy> repository)
    {
        public async Task<Result<BookCopy>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<BookCopy>.Failure(
                    Error.NotFound("BookCopy was not found."));
            }

            return Result<BookCopy>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/library/book-copy/{id:guid}",
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
            .WithName("GetBookCopyById")
            .WithTags("Library")
            .RequireAuthorization();

        return endpoints;
    }
}
