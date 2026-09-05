using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Models;

/// <summary>Represents a recipient-specific school notification.</summary>
public sealed class NotificationEntity : Entity
{
    /// <summary>Gets the entity-specific identifier.</summary>
    public Guid NotificationId { get; private set; } = Guid.NewGuid();

    private NotificationEntity()
    {
    }

    /// <summary>Gets the recipient user identifier.</summary>
    public Guid RecipientUserId { get; private set; }

    /// <summary>Gets the notification type.</summary>
    public NotificationType Type { get; private set; }

    /// <summary>Gets the notification title.</summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>Gets the notification body.</summary>
    public string Message { get; private set; } = string.Empty;

    /// <summary>Gets the related business entity identifier.</summary>
    public Guid? RelatedEntityId { get; private set; }

    /// <summary>Gets the related business entity type.</summary>
    public string? RelatedEntityType { get; private set; }

    /// <summary>Gets the optional application route.</summary>
    public string? ActionUrl { get; private set; }

    /// <summary>Gets the notification priority.</summary>
    public string Priority { get; private set; } = "Normal";

    /// <summary>Gets whether it has been read.</summary>
    public bool IsRead { get; private set; }

    /// <summary>Gets when it was read.</summary>
    public DateTimeOffset? ReadAt { get; private set; }

    /// <summary>Gets when the business event occurred.</summary>
    public DateTimeOffset OccurredAt { get; private set; }

    /// <summary>Creates a notification.</summary>
    public static NotificationEntity Create(
        Guid tenantId,
        Guid recipientUserId,
        NotificationType type,
        string title,
        string message,
        Guid? relatedEntityId = null,
        string? relatedEntityType = null,
        string? actionUrl = null,
        string priority = "Normal")
    {
        return new NotificationEntity
        {
            TenantId = tenantId,
            RecipientUserId = recipientUserId,
            Type = type,
            Title = title,
            Message = message,
            RelatedEntityId = relatedEntityId,
            RelatedEntityType = relatedEntityType,
            ActionUrl = actionUrl,
            Priority = priority,
            OccurredAt = DateTimeOffset.UtcNow
        };
    }

    /// <summary>
    /// Updates the editable notification details.
    /// </summary>
    /// <param name="type">Notification type.</param>
    /// <param name="title">Notification title.</param>
    /// <param name="message">Notification message.</param>
    /// <param name="relatedEntityId">Related business entity identifier.</param>
    /// <param name="relatedEntityType">Related business entity type.</param>
    /// <param name="actionUrl">Application route associated with the notification.</param>
    /// <param name="priority">Notification priority.</param>
    public void UpdateDetails(
        NotificationType type,
        string title,
        string message,
        Guid? relatedEntityId,
        string? relatedEntityType,
        string? actionUrl,
        string priority)
    {
        Type = type;
        Title = title;
        Message = message;
        RelatedEntityId = relatedEntityId;
        RelatedEntityType = relatedEntityType;
        ActionUrl = actionUrl;
        Priority = priority;
    }

    /// <summary>Marks the notification as read.</summary>
    public void MarkAsRead()
    {
        IsRead = true;
        ReadAt = DateTimeOffset.UtcNow;
    }
}
