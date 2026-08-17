using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AICore.Contracts;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.Modules.AICore.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AICore.Features.KnowledgeChunk;

public static class GetKnowledgeChunkById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<KnowledgeChunkResponse>>;

    public sealed class Handler(IKnowledgeChunkQuery entityQuery)
        : IRequestHandler<Query, Result<KnowledgeChunkResponse>>
    {
        public async Task<Result<KnowledgeChunkResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<KnowledgeChunkResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(KnowledgeChunk))));
            }
            return Result<KnowledgeChunkResponse>.Success(KnowledgeChunkResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "knowledge-chunk"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<KnowledgeChunkResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetKnowledgeChunkById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
