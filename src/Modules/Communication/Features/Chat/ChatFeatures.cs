using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Features.Chat;

/// <summary>Chat read port.</summary>
public interface IChatQuery
{
    Task<IReadOnlyCollection<ConversationResponse>> GetConversationsAsync(Guid tenantId, Guid userId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<MessageResponse>> GetMessagesAsync(Guid tenantId, Guid conversationId, Guid userId, CancellationToken cancellationToken);
}

/// <summary>Chat write port.</summary>
public interface IChatCommand
{
    Task<ChatConversationEntity> CreateConversationAsync(Guid tenantId, string title, string type, IReadOnlyCollection<ParticipantRequest> participants, CancellationToken cancellationToken);
    Task<ChatMessageEntity> SendMessageAsync(Guid tenantId, Guid conversationId, Guid senderUserId, string message, CancellationToken cancellationToken);
}

/// <summary>Participant input.</summary>
public sealed record ParticipantRequest(Guid UserId, string Role);
/// <summary>Conversation list DTO.</summary>
public sealed record ConversationResponse(Guid TenantId, Guid Id, string Title, string ConversationType, string? LastMessage, DateTimeOffset? LastMessageAt, int UnreadCount);
/// <summary>Message DTO.</summary>
public sealed record MessageResponse(Guid TenantId, Guid Id, Guid ConversationId, Guid SenderUserId, string SenderDisplayName, string SenderRole, string Message, DateTimeOffset SentAt);

/// <summary>Send-message feature.</summary>
public static class SendMessage
{
    /// <summary>Request.</summary>
    public sealed record Request(Guid TenantId, Guid ConversationId, Guid SenderUserId, string Message);
    /// <summary>Response.</summary>
    public sealed record Response(Guid TenantId, Guid Id, Guid ConversationId, string Message, DateTimeOffset SentAt);
    /// <summary>Handler.</summary>
    public sealed class Handler(IChatCommand command)
    {
        /// <summary>Handles a message.</summary>
        public async Task<Result<Response>> Handle(Request request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
                return Result<Response>.Failure(Error.Validation("Message is required."));
            var entity=await command.SendMessageAsync(request.TenantId,request.ConversationId,request.SenderUserId,request.Message.Trim(),cancellationToken);
            return Result<Response>.Success(new(entity.TenantId,entity.ChatMessageId,entity.ConversationId,entity.Message,entity.SentAt));
        }
    }
}
