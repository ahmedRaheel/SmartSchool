using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.Modules.Communication.Persistence;
using SmartSchool.Modules.Communication.Realtime;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Communication.Features.Chat.SendChatMessage;

public static class SendChatMessage
{
    public sealed record Body(string Message);
    public sealed record Request(Guid ConversationId, string Message) : IRequest<Response?>;
    public sealed record Response(Guid TenantId, Guid MessageId, Guid ConversationId, Guid SenderUserId, string Message, DateTimeOffset SentAt);
    public interface ISendChatMessageCommand { Task<ChatMessageEntity?> ExecuteAsync(Guid? tenantId, bool isSuperAdmin, Guid userId, Request request, CancellationToken cancellationToken); }
    internal sealed class SendChatMessageCommand(ICommunicationDbContext dbContext) : ISendChatMessageCommand
    {
        public async Task<ChatMessageEntity?> ExecuteAsync(Guid? tenantId, bool isSuperAdmin, Guid userId, Request request, CancellationToken cancellationToken)
        {
            var conversation = await dbContext.ChatConversations.SingleOrDefaultAsync(x => x.ChatConversationId == request.ConversationId && x.IsActive, cancellationToken);
            if (conversation is null || (!isSuperAdmin && conversation.TenantId != tenantId)) return null;
            if (!isSuperAdmin && !await dbContext.ChatParticipants.AnyAsync(x => x.TenantId == conversation.TenantId && x.ConversationId == request.ConversationId && x.UserId == userId && x.IsActive, cancellationToken)) return null;
            var entity = ChatMessageEntity.Create(conversation.TenantId, request.ConversationId, userId, request.Message.Trim());
            await dbContext.ChatMessages.AddAsync(entity, cancellationToken); await dbContext.SaveChangesAsync(cancellationToken); return entity;
        }
    }
    public sealed class Handler(ITenantScope tenantScope, ISendChatMessageCommand command, IIntegrationEventPublisher events, IHubContext<ChatHub> hub) : IRequestHandler<Request, Response?>
    {
        public async Task<Response?> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Message)) return null;
            var entity = await command.ExecuteAsync(tenantScope.TenantId, tenantScope.IsSuperAdmin, tenantScope.UserId, request, cancellationToken); if (entity is null) return null;
            var response = new Response(entity.TenantId, entity.ChatMessageId, entity.ConversationId, entity.SenderUserId, entity.Message, entity.SentAt);
            await events.PublishAsync(KafkaTopics.ChatMessageSent, response, cancellationToken);
            await hub.Clients.Group(CommunicationGroups.Conversation(entity.TenantId, entity.ConversationId)).SendAsync("MessageReceived", response, cancellationToken);
            return response;
        }
    }
    public static void MapEndpoint(IEndpointRouteBuilder endpoints) => endpoints.MapPost("/api/communication/chat/conversations/{conversationId:guid}/messages", async (Guid conversationId, Body body, IMediator mediator, CancellationToken cancellationToken) =>
    {
        var response = await mediator.SendAsync<Request, Response?>(new Request(conversationId, body.Message), cancellationToken);
        return response is null ? Results.BadRequest() : Results.Ok(response);
    }).WithTags("Communication - Chat").RequireAuthorization();
}
