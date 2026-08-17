using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIParent.Contracts;
using SmartSchool.Modules.AIParent.Models;
using SmartSchool.Modules.AIParent.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIParent.Features.ParentConversation;

public static class GetParentConversationById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<ParentConversationResponse>>;

    public sealed class Handler(IParentConversationQuery entityQuery)
        : IRequestHandler<Query, Result<ParentConversationResponse>>
    {
        public async Task<Result<ParentConversationResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<ParentConversationResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(ParentConversation))));
            }
            return Result<ParentConversationResponse>.Success(ParentConversationResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "parent-conversation"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<ParentConversationResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetParentConversationById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
