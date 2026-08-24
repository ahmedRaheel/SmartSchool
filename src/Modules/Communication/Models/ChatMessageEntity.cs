using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Models;

/// <summary>Represents a persisted chat message.</summary>
public sealed class ChatMessageEntity : Entity
{
<<<<<<< HEAD
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid ChatMessageId { get; private set; } = Guid.NewGuid();
private ChatMessageEntity()
    {
    }
=======
	/// <summary>Gets the persisted entity identifier.</summary>
	public Guid Id
	{
		get => Id;
		private set => Id = value;
	}

    private ChatMessageEntity() { }
>>>>>>> c40f31f829a59dcdb7fd9fe0046a26e6e366eca0

    /// <summary>Gets the conversation identifier.</summary>
    public Guid ConversationId { get; private set; }

    /// <summary>Gets the sender user identifier.</summary>
    public Guid SenderUserId { get; private set; }

    /// <summary>Gets the message type.</summary>
    public string MessageType { get; private set; } = "Text";

    /// <summary>Gets the message body.</summary>
    public string Message { get; private set; } = string.Empty;

    /// <summary>Gets the optional message being replied to.</summary>
    public Guid? ReplyToMessageId { get; private set; }

    /// <summary>Gets the UTC send time.</summary>
    public DateTimeOffset SentAt { get; private set; }

    /// <summary>Gets the edit time.</summary>
    public DateTimeOffset? EditedAt { get; private set; }

    /// <summary>Gets whether the message was deleted.</summary>
    public bool IsDeleted { get; private set; }

    /// <summary>Creates a message.</summary>
    public static ChatMessageEntity Create(
        Guid tenantId,
        Guid conversationId,
        Guid senderUserId,
        string message,
        Guid? replyToMessageId = null)
    {
        return new ChatMessageEntity
        {
            TenantId = tenantId,
            ConversationId = conversationId,
            SenderUserId = senderUserId,
            Message = message,
            ReplyToMessageId = replyToMessageId,
            SentAt = DateTimeOffset.UtcNow
        };
    }

    /// <summary>Edits the message text.</summary>
    public void Edit(string message)
    {
        Message = message;
        EditedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>Soft deletes the message.</summary>
    public void Delete() => IsDeleted = true;
}
