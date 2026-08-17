using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Communication.Contracts;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.Modules.Communication.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Communication.Features.Conversation;

public static class GetConversationById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<ConversationResponse>>;

    public sealed class Handler(IConversationQuery entityQuery)
        : IRequestHandler<Query, Result<ConversationResponse>>
    {
        public async Task<Result<ConversationResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<ConversationResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Conversation))));
            }
            return Result<ConversationResponse>.Success(ConversationResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "conversation"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<ConversationResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetConversationById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
