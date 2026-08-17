using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Library.Contracts;
using SmartSchool.Modules.Library.Models;
using SmartSchool.Modules.Library.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Library.Features.BookCopy;

public static class GetBookCopyById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<BookCopyResponse>>;

    public sealed class Handler(IBookCopyQuery entityQuery)
        : IRequestHandler<Query, Result<BookCopyResponse>>
    {
        public async Task<Result<BookCopyResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<BookCopyResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(BookCopy))));
            }
            return Result<BookCopyResponse>.Success(BookCopyResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "book-copy"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<BookCopyResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetBookCopyById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
