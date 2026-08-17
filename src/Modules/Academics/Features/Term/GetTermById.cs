using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Academics.Contracts;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.Modules.Academics.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Academics.Features.Term;

public static class GetTermById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<TermResponse>>;

    public sealed class Handler(ITermQuery entityQuery)
        : IRequestHandler<Query, Result<TermResponse>>
    {
        public async Task<Result<TermResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<TermResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Term))));
            }
            return Result<TermResponse>.Success(TermResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "term"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<TermResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetTermById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
