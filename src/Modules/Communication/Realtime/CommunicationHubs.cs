using System.Threading.Tasks;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Communication.Realtime;

internal static class CommunicationGroups
{
    public static string User(Guid tenantId, Guid userId) => $"tenant:{tenantId}:user:{userId}";
    public static string Conversation(Guid tenantId, Guid conversationId) => $"tenant:{tenantId}:conversation:{conversationId}";
}

[Authorize]
public sealed class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var tenantId = GetGuidClaim(SmartSchoolClaims.TenantId);
        var userId = GetGuidClaim(SmartSchoolClaims.UserId);
        await Groups.AddToGroupAsync(Context.ConnectionId, CommunicationGroups.User(tenantId, userId));
        await base.OnConnectedAsync();
    }

    private Guid GetGuidClaim(string claimType)
    {
        var value = Context.User?.FindFirstValue(claimType);
        return Guid.TryParse(value, out var id) ? id : throw new HubException($"Required claim '{claimType}' is missing.");
    }
}

[Authorize]
public sealed class ChatHub : Hub
{
    public Task JoinConversation(Guid tenantId, Guid conversationId)
    {
        var tokenTenant = Context.User?.FindFirstValue(SmartSchoolClaims.TenantId);
        var isSuperAdmin = Context.User?.IsInRole(SmartSchoolRoles.SuperAdmin) == true;
        if (!isSuperAdmin && !string.Equals(tokenTenant, tenantId.ToString(), StringComparison.OrdinalIgnoreCase))
            throw new HubException("Conversation tenant is outside the authenticated tenant scope.");
        return Groups.AddToGroupAsync(Context.ConnectionId, CommunicationGroups.Conversation(tenantId, conversationId));
    }

    public async Task SendMessage(Guid tenantId, Guid conversationId, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new HubException("Message is required.");

        var tokenTenant = Context.User?.FindFirstValue(SmartSchoolClaims.TenantId);
        var isSuperAdmin = Context.User?.IsInRole(SmartSchoolRoles.SuperAdmin) == true;
        if (!isSuperAdmin && !string.Equals(tokenTenant, tenantId.ToString(), StringComparison.OrdinalIgnoreCase))
            throw new HubException("Conversation tenant is outside the authenticated tenant scope.");

        var senderUserId = Context.User?.FindFirstValue(SmartSchoolClaims.UserId);
        var senderRole = Context.User?.FindFirstValue(SmartSchoolClaims.Role);
        var payload = new
        {
            tenantId,
            conversationId,
            senderUserId,
            senderRole,
            message = message.Trim(),
            sentAt = DateTimeOffset.UtcNow
        };

        await Clients.Group(CommunicationGroups.Conversation(tenantId, conversationId))
            .SendAsync("MessageReceived", payload);
    }

    public Task LeaveConversation(Guid tenantId, Guid conversationId) =>
        Groups.RemoveFromGroupAsync(Context.ConnectionId, CommunicationGroups.Conversation(tenantId, conversationId));
}
