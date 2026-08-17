using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Library.Contracts;
using SmartSchool.Modules.Library.Models;
using SmartSchool.Modules.Library.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Library.Features.Book;

public static class GetBookById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<BookResponse>>;

    public sealed class Handler(IBookQuery entityQuery)
        : IRequestHandler<Query, Result<BookResponse>>
    {
        public async Task<Result<BookResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<BookResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Book))));
            }
            return Result<BookResponse>.Success(BookResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "book"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<BookResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetBookById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
