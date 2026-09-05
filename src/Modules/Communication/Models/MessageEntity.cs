using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Models;

/// <summary>
/// Represents the MessageEntity domain entity.
/// </summary>
public sealed class MessageEntity : Entity
{
    /// <summary>Gets the entity-specific identifier.</summary>
    public Guid MessageId { get; private set; } = Guid.NewGuid();

    private MessageEntity()
    {
    }

    /// <summary>Gets the persisted conversation id value.</summary>
    public Guid ConversationId { get; private set; }

    /// <summary>Gets the persisted sender user id value.</summary>
    public Guid SenderUserId { get; private set; }

    /// <summary>Gets the persisted reply to message id value.</summary>
    public Guid? ReplyToMessageId { get; private set; }

    /// <summary>Gets the persisted message type code value.</summary>
    public string MessageTypeCode { get; private set; } = string.Empty;

    /// <summary>Gets the persisted body value.</summary>
    public string? Body { get; private set; }

    /// <summary>Gets the persisted sent at value.</summary>
    public DateTimeOffset SentAt { get; private set; }

    /// <summary>Gets the persisted edited at value.</summary>
    public DateTimeOffset? EditedAt { get; private set; }

    /// <summary>Gets the persisted deleted at value.</summary>
    public DateTimeOffset? DeletedAt { get; private set; }

    /// <summary>Gets the business code.</summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>Gets the display name.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets optional domain metadata serialized as JSON.</summary>
    public string? MetadataJson { get; private set; }

    /// <summary>Creates a new MessageEntity.</summary>
    /// <param name="tenantId">The owning tenant identifier.</param>
    /// <param name="code">The business code.</param>
    /// <param name="name">The display name.</param>
    /// <param name="metadataJson">Optional domain metadata.</param>
    /// <returns>The newly created entity.</returns>
    public static MessageEntity Create(
        Guid tenantId,
        string code,
        string name,
        string? metadataJson = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new MessageEntity
        {
            TenantId = tenantId,
            Code = code.Trim(),
            Name = name.Trim(),
            MetadataJson = metadataJson
        };
    }

    /// <summary>Updates the business details.</summary>
    /// <param name="code">The new business code.</param>
    /// <param name="name">The new display name.</param>
    /// <param name="metadataJson">Optional domain metadata.</param>
    public void UpdateDetails(
        string code,
        string name,
        string? metadataJson = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Code = code.Trim();
        Name = name.Trim();
        MetadataJson = metadataJson;
        MarkAsUpdated();
    }
}
