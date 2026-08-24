using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Models;

/// <summary>Represents a tenant-scoped conversation between school users.</summary>
public sealed class ChatConversationEntity : Entity
{
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid ChatConversationId { get; private set; } = Guid.NewGuid();
private ChatConversationEntity()
    {
    }

    /// <summary>Gets the conversation title.</summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>Gets the conversation category.</summary>
    public string ConversationType { get; private set; } = string.Empty;

    /// <summary>Gets the user who created the conversation.</summary>
    public Guid CreatedByUserId { get; private set; }

    /// <summary>Gets the optional business entity related to the conversation.</summary>
    public Guid? RelatedEntityId { get; private set; }

    /// <summary>Gets the optional related entity type.</summary>
    public string? RelatedEntityType { get; private set; }

    /// <summary>Gets whether the conversation has been closed.</summary>
    public bool IsClosed { get; private set; }

    /// <summary>Gets when the conversation was closed.</summary>
    public DateTimeOffset? ClosedAt { get; private set; }

    /// <summary>Creates a conversation.</summary>
    public static ChatConversationEntity Create(
        Guid tenantId,
        string title,
        string conversationType,
        Guid createdByUserId,
        Guid? relatedEntityId = null,
        string? relatedEntityType = null)
    {
        return new ChatConversationEntity
        {
            TenantId = tenantId,
            Title = title,
            ConversationType = conversationType,
            CreatedByUserId = createdByUserId,
            RelatedEntityId = relatedEntityId,
            RelatedEntityType = relatedEntityType
        };
    }

    /// <summary>Closes the conversation.</summary>
    public void Close()
    {
        IsClosed = true;
        ClosedAt = DateTimeOffset.UtcNow;
    }
}
