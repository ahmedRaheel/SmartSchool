using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Models;

/// <summary>Stores a user's delivery preferences for a notification type.</summary>
public sealed class NotificationPreferenceEntity : Entity
{
	/// <summary>Gets the persisted entity identifier.</summary>
	public Guid Id
	{
		get => Id;
		private set => Id = value;
	}

    private NotificationPreferenceEntity() { }

    /// <summary>Gets the user identifier.</summary>
    public Guid UserId { get; private set; }

    /// <summary>Gets the notification type.</summary>
    public NotificationType NotificationType { get; private set; }

    /// <summary>Gets whether in-app delivery is enabled.</summary>
    public bool InAppEnabled { get; private set; } = true;

    /// <summary>Gets whether push delivery is enabled.</summary>
    public bool PushEnabled { get; private set; } = true;

    /// <summary>Gets whether email delivery is enabled.</summary>
    public bool EmailEnabled { get; private set; }

    /// <summary>Gets whether SMS delivery is enabled.</summary>
    public bool SmsEnabled { get; private set; }
}
