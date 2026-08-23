using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Communication.Realtime;

public static class CommunicationGroups
{
    public static string User(Guid tenantId, Guid userId) => $"tenant:{tenantId}:user:{userId}";
    public static string Conversation(Guid tenantId, Guid conversationId) => $"tenant:{tenantId}:conversation:{conversationId}";
}

[Authorize]
public sealed class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = RequiredGuid(SmartSchoolClaims.UserId);
        var tenant = OptionalGuid(SmartSchoolClaims.TenantId);
        if (tenant.HasValue)
            await Groups.AddToGroupAsync(Context.ConnectionId, CommunicationGroups.User(tenant.Value, userId));
        await base.OnConnectedAsync();
    }
    private Guid RequiredGuid(string type) => OptionalGuid(type) ?? throw new HubException($"Required claim '{type}' is missing.");
    private Guid? OptionalGuid(string type) => Guid.TryParse(Context.User?.FindFirstValue(type), out var id) ? id : null;
}

[Authorize]
public sealed class ChatHub(IApplicationDbContext dbContext, IIntegrationEventPublisher publisher) : Hub
{
    public async Task JoinConversation(Guid conversationId)
    {
        var scope = await ResolveMembershipAsync(conversationId, Context.ConnectionAborted);
        await Groups.AddToGroupAsync(Context.ConnectionId, CommunicationGroups.Conversation(scope.TenantId, conversationId));
    }

    public async Task SendMessage(Guid conversationId, string message)
    {
        if (string.IsNullOrWhiteSpace(message)) throw new HubException("Message is required.");
        var scope = await ResolveMembershipAsync(conversationId, Context.ConnectionAborted);
        var entity = ChatMessageEntity.Create(scope.TenantId, conversationId, scope.UserId, message.Trim());
        await dbContext.Set<ChatMessageEntity>().AddAsync(entity, Context.ConnectionAborted);
        await dbContext.SaveChangesAsync(Context.ConnectionAborted);

        var payload = new { entity.TenantId, entity.Id, entity.ConversationId, entity.SenderUserId, entity.Message, entity.SentAt };
        await publisher.PublishAsync(KafkaTopics.ChatMessageSent, payload, Context.ConnectionAborted);
        await Clients.Group(CommunicationGroups.Conversation(scope.TenantId, conversationId))
            .SendAsync("MessageReceived", payload, Context.ConnectionAborted);
    }

    public async Task LeaveConversation(Guid conversationId)
    {
        var scope = await ResolveMembershipAsync(conversationId, Context.ConnectionAborted);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, CommunicationGroups.Conversation(scope.TenantId, conversationId));
    }

    private async Task<(Guid TenantId, Guid UserId)> ResolveMembershipAsync(Guid conversationId, CancellationToken ct)
    {
        var userId = RequiredGuid(SmartSchoolClaims.UserId);
        var isSuperAdmin = Context.User?.IsInRole(SmartSchoolRoles.SuperAdmin) == true;
        var conversation = await dbContext.Set<ChatConversationEntity>().AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == conversationId && x.IsActive, ct)
            ?? throw new HubException("Conversation was not found.");

        if (!isSuperAdmin)
        {
            var tokenTenant = RequiredGuid(SmartSchoolClaims.TenantId);
            if (conversation.TenantId != tokenTenant) throw new HubException("Conversation is outside your tenant.");
            var member = await dbContext.Set<ChatParticipantEntity>().AsNoTracking()
                .AnyAsync(x => x.TenantId == tokenTenant && x.ConversationId == conversationId && x.UserId == userId && x.IsActive, ct);
            if (!member) throw new HubException("You are not a participant in this conversation.");
        }
        return (conversation.TenantId, userId);
    }
    private Guid RequiredGuid(string type) => Guid.TryParse(Context.User?.FindFirstValue(type), out var id) ? id : throw new HubException($"Required claim '{type}' is missing.");
}
