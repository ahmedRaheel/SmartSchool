using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Communication.Contracts;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.Modules.Communication.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Communication.Features.ConversationParticipant;

public static class GetConversationParticipantById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<ConversationParticipantResponse>>;

    public sealed class Handler(IConversationParticipantQuery entityQuery)
        : IRequestHandler<Query, Result<ConversationParticipantResponse>>
    {
        public async Task<Result<ConversationParticipantResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<ConversationParticipantResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(ConversationParticipant))));
            }
            return Result<ConversationParticipantResponse>.Success(ConversationParticipantResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "conversation-participant"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<ConversationParticipantResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetConversationParticipantById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
