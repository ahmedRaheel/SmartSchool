using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Communication.Realtime;

public static class CommunicationGroups
{
    public static string User(Guid tenantId, Guid userId)
    {
        return $"tenant:{tenantId}:user:{userId}";
    }

    public static string Conversation(Guid tenantId, Guid conversationId)
    {
        return $"tenant:{tenantId}:conversation:{conversationId}";
    }
}

[Authorize]
public sealed class NotificationHub(ICurrentUser currentUser) : Hub
{
    public override async Task OnConnectedAsync()
    {
        if (!currentUser.TenantId.HasValue)
        {
            await base.OnConnectedAsync();
            return;
        }

        var groupName = CommunicationGroups.User(
            currentUser.TenantId.Value,
            currentUser.UserId);

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await base.OnConnectedAsync();
    }
}

[Authorize]
public sealed class ChatHub(
    IApplicationDbContext dbContext,
    IIntegrationEventPublisher publisher,
    ICurrentUser currentUser) : Hub
{
    public async Task JoinConversation(Guid conversationId)
    {
        var (tenantId, _) = await ResolveMembershipAsync(
            conversationId,
            Context.ConnectionAborted);

        var groupName = CommunicationGroups.Conversation(
            tenantId,
            conversationId);

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task SendMessage(Guid conversationId, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new HubException("Message is required.");
        }

        var (tenantId, userId) = await ResolveMembershipAsync(
            conversationId,
            Context.ConnectionAborted);

        var entity = ChatMessageEntity.Create(
            tenantId,
            conversationId,
            userId,
            message.Trim());

        await dbContext.Set<ChatMessageEntity>().AddAsync(
            entity,
            Context.ConnectionAborted);

        await dbContext.SaveChangesAsync(Context.ConnectionAborted);

        var payload = new
        {
            entity.TenantId,
            entity.ChatMessageId,
            entity.ConversationId,
            entity.SenderUserId,
            entity.Message,
            entity.SentAt
        };

        await publisher.PublishAsync(
            KafkaTopics.ChatMessageSent,
            payload,
            Context.ConnectionAborted);

        var groupName = CommunicationGroups.Conversation(
            tenantId,
            conversationId);

        await Clients
            .Group(groupName)
            .SendAsync(
                "MessageReceived",
                payload,
                Context.ConnectionAborted);
    }

    public async Task LeaveConversation(Guid conversationId)
    {
        var (tenantId, _) = await ResolveMembershipAsync(
            conversationId,
            Context.ConnectionAborted);

        var groupName = CommunicationGroups.Conversation(
            tenantId,
            conversationId);

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

    private async Task<(Guid TenantId, Guid UserId)> ResolveMembershipAsync(
        Guid conversationId,
        CancellationToken cancellationToken)
    {
        var conversation = await dbContext
            .Set<ChatConversationEntity>()
            .AsNoTracking()
            .SingleOrDefaultAsync(
                item => item.ChatConversationId == conversationId && item.IsActive,
                cancellationToken);

        if (conversation is null)
        {
            throw new HubException("Conversation was not found.");
        }

        if (!currentUser.IsSuperAdmin)
        {
            if (!currentUser.TenantId.HasValue)
            {
                throw new HubException("Tenant context is missing from the access token.");
            }

            if (conversation.TenantId != currentUser.TenantId.Value)
            {
                throw new HubException("Conversation is outside your tenant.");
            }

            var isParticipant = await dbContext
                .Set<ChatParticipantEntity>()
                .AsNoTracking()
                .AnyAsync(
                    item =>
                        item.TenantId == currentUser.TenantId.Value &&
                        item.ConversationId == conversationId &&
                        item.UserId == currentUser.UserId &&
                        item.IsActive,
                    cancellationToken);

            if (!isParticipant)
            {
                throw new HubException("You are not a participant in this conversation.");
            }
        }

        return (conversation.TenantId, currentUser.UserId);
    }
}
