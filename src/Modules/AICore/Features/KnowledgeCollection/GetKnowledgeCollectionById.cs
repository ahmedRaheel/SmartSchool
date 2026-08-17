using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AICore.Contracts;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.Modules.AICore.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AICore.Features.KnowledgeCollection;

public static class GetKnowledgeCollectionById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<KnowledgeCollectionResponse>>;

    public sealed class Handler(IKnowledgeCollectionQuery entityQuery)
        : IRequestHandler<Query, Result<KnowledgeCollectionResponse>>
    {
        public async Task<Result<KnowledgeCollectionResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<KnowledgeCollectionResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(KnowledgeCollection))));
            }
            return Result<KnowledgeCollectionResponse>.Success(KnowledgeCollectionResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "knowledge-collection"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<KnowledgeCollectionResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetKnowledgeCollectionById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
