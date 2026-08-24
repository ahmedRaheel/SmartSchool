using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Models;

/// <summary>Represents a user's membership in a chat conversation.</summary>
public sealed class ChatParticipantEntity : Entity
{
<<<<<<< HEAD
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid ChatParticipantId { get; private set; } = Guid.NewGuid();
private ChatParticipantEntity()
    {
    }
=======
	/// <summary>Gets the persisted entity identifier.</summary>
	public Guid Id
	{
		get => Id;
		private set => Id = value;
	}

    private ChatParticipantEntity() { }
>>>>>>> c40f31f829a59dcdb7fd9fe0046a26e6e366eca0

    /// <summary>Gets the conversation identifier.</summary>
    public Guid ConversationId { get; private set; }

    /// <summary>Gets the authenticated user identifier.</summary>
    public Guid UserId { get; private set; }

    /// <summary>Gets the participant role.</summary>
    public string Role { get; private set; } = string.Empty;

    /// <summary>Gets when the participant joined.</summary>
    public DateTimeOffset JoinedAt { get; private set; }

    /// <summary>Gets when the participant last read the conversation.</summary>
    public DateTimeOffset? LastReadAt { get; private set; }

    /// <summary>Gets whether the participant muted notifications.</summary>
    public bool IsMuted { get; private set; }

    /// <summary>Creates a participant.</summary>
    public static ChatParticipantEntity Create(
        Guid tenantId,
        Guid conversationId,
        Guid userId,
        string role)
    {
        return new ChatParticipantEntity
        {
            TenantId = tenantId,
            ConversationId = conversationId,
            UserId = userId,
            Role = role,
            JoinedAt = DateTimeOffset.UtcNow
        };
    }

    /// <summary>Marks the conversation as read for this participant.</summary>
    public void MarkRead() => LastReadAt = DateTimeOffset.UtcNow;

    /// <summary>Changes notification mute state.</summary>
    public void SetMuted(bool isMuted) => IsMuted = isMuted;
}
