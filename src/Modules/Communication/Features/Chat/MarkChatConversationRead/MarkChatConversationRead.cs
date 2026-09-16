using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Communication.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Features.Chat.MarkChatConversationRead;

public static class MarkChatConversationRead
{
    public sealed record Request(Guid ConversationId) : IRequest<Result>;

    public interface IMarkChatConversationRead
    {
        Task<bool> ExecuteAsync(Guid? tenantId, Guid userId, Guid conversationId, CancellationToken cancellationToken);
    }

    internal sealed class MarkChatConversationReadCommand(ICommunicationDbContext dbContext) : IMarkChatConversationRead
    {
        public async Task<bool> ExecuteAsync(Guid? tenantId, Guid userId, Guid conversationId, CancellationToken cancellationToken)
        {
            var participant = await dbContext.ChatParticipants.SingleOrDefaultAsync(
                x => x.ConversationId == conversationId && x.UserId == userId && x.IsActive && (!tenantId.HasValue || x.TenantId == tenantId.Value),
                cancellationToken);
            if (participant is null)
            {
                return false;
            }

            participant.MarkRead();
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

    public sealed class Handler(ITenantScope tenantScope, IMarkChatConversationRead command) : IRequestHandler<Request, Result>
    {
        public async Task<Result> HandleAsync(Request request, CancellationToken cancellationToken) =>
            await command.ExecuteAsync(tenantScope.TenantId, tenantScope.UserId, request.ConversationId, cancellationToken)
                ? Result.Success()
                : Result.Failure(Error.NotFound("Conversation membership was not found."));
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/api/communication/chat/conversations/{conversationId:guid}/read", async (Guid conversationId, IMediator mediator, CancellationToken cancellationToken) =>
                (await mediator.SendAsync<Request, Result>(new Request(conversationId), cancellationToken)).ToHttpResult())
            .WithTags("Communication - Chat")
            .RequireAuthorization();
}
