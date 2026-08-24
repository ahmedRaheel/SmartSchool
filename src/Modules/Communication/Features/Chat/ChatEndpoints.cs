using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Application.Identity;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.Modules.Communication.Realtime;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Communication.Features.Chat;

public static class ChatEndpoints
{
    public sealed record CreateConversationRequest(Guid? TenantId,string Title,string Type,IReadOnlyCollection<ParticipantRequest> Participants);
    public sealed record SendRequest(string Message);

    public static IEndpointRouteBuilder MapChatEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group=endpoints.MapGroup("/api/communication/chat").WithTags("Communication - Chat").RequireAuthorization();
        group.MapGet("/conversations", ConversationsAsync);
        group.MapGet("/conversations/{conversationId:guid}/messages", MessagesAsync);
        group.MapPost("/conversations", CreateAsync);
        group.MapPost("/conversations/{conversationId:guid}/messages", SendAsync);
        return endpoints;
    }

    private static async Task<IResult> ConversationsAsync(ITenantScope scope,IApplicationDbContext db,CancellationToken ct)
    {
        var userId=scope.UserId; var tenant=scope.TenantId;
        var participantIds=db.Set<ChatParticipantEntity>().AsNoTracking().Where(x=>x.UserId==userId && x.IsActive && (!tenant.HasValue || x.TenantId==tenant.Value)).Select(x=>x.ConversationId);
        var rows=await db.Set<ChatConversationEntity>().AsNoTracking().Where(x=>participantIds.Contains(x.ChatConversationId) && x.IsActive)
            .OrderByDescending(x=>x.CreatedAt).Select(x=>new { x.TenantId, x.ChatConversationId, x.Title,x.ConversationType,x.CreatedByUserId,x.IsClosed }).ToListAsync(ct);
        return Results.Ok(rows);
    }

    private static async Task<IResult> MessagesAsync(Guid conversationId,ITenantScope scope,IApplicationDbContext db,CancellationToken ct)
    {
        var conversation=await AuthorizedConversation(conversationId,scope,db,ct); if(conversation is null)return Results.Forbid();
        var rows=await db.Set<ChatMessageEntity>().AsNoTracking().Where(x=>x.ConversationId==conversationId && x.TenantId==conversation.TenantId && x.IsActive && !x.IsDeleted)
            .OrderBy(x=>x.SentAt).Select(x=>new{x.TenantId, x.ChatMessageId, x.ConversationId,x.SenderUserId,x.Message,x.SentAt,x.EditedAt}).ToListAsync(ct);
        return Results.Ok(rows);
    }

    private static async Task<IResult> CreateAsync(CreateConversationRequest request,ITenantScope scope,IApplicationDbContext db,CancellationToken ct)
    {
        var tenant=scope.IsSuperAdmin ? request.TenantId : scope.Resolve(request.TenantId);
        if(!tenant.HasValue)return Results.BadRequest(new{message="A tenant is required."});
        if(string.IsNullOrWhiteSpace(request.Title))return Results.BadRequest(new{message="Title is required."});
        var entity=ChatConversationEntity.Create(tenant.Value,request.Title.Trim(),request.Type,scope.UserId);
        await db.Set<ChatConversationEntity>().AddAsync(entity,ct);
        var participants=request.Participants.Append(new ParticipantRequest(scope.UserId,"Creator")).GroupBy(x=>x.UserId).Select(x=>x.First());
        foreach(var p in participants) await db.Set<ChatParticipantEntity>().AddAsync(ChatParticipantEntity.Create(tenant.Value,entity.ChatConversationId, p.UserId,p.Role),ct);
        await db.SaveChangesAsync(ct); return Results.Created($"/api/communication/chat/conversations/{entity.ChatConversationId}",new{entity.TenantId,entity.ChatConversationId, entity.Title,entity.ConversationType});
    }

    private static async Task<IResult> SendAsync(Guid conversationId,SendRequest request,ITenantScope scope,IApplicationDbContext db,IIntegrationEventPublisher events,IHubContext<ChatHub> hub,CancellationToken ct)
    {
        if(string.IsNullOrWhiteSpace(request.Message))return Results.BadRequest(new{message="Message is required."});
        var conversation=await AuthorizedConversation(conversationId,scope,db,ct); if(conversation is null)return Results.Forbid();
        var entity=ChatMessageEntity.Create(conversation.TenantId,conversationId,scope.UserId,request.Message.Trim()); await db.Set<ChatMessageEntity>().AddAsync(entity,ct); await db.SaveChangesAsync(ct);
        var payload=new{entity.TenantId,entity.ChatMessageId, entity.ConversationId,entity.SenderUserId,entity.Message,entity.SentAt};
        await events.PublishAsync(KafkaTopics.ChatMessageSent,payload,ct); await hub.Clients.Group(CommunicationGroups.Conversation(entity.TenantId,conversationId)).SendAsync("MessageReceived",payload,ct);
        return Results.Ok(payload);
    }

    private static async Task<ChatConversationEntity?> AuthorizedConversation(Guid id,ITenantScope scope,IApplicationDbContext db,CancellationToken ct)
    {
        var c=await db.Set<ChatConversationEntity>().AsNoTracking().SingleOrDefaultAsync(x=>x.ChatConversationId==id&&x.IsActive,ct); if(c is null)return null;
        if(!scope.IsSuperAdmin && c.TenantId!=scope.TenantId)return null;
        if(scope.IsSuperAdmin)return c;
        return await db.Set<ChatParticipantEntity>().AsNoTracking().AnyAsync(x=>x.TenantId==c.TenantId&&x.ConversationId==id&&x.UserId==scope.UserId&&x.IsActive,ct)?c:null;
    }
}
