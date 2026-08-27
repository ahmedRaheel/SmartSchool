using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Models;

/// <summary>
/// Represents the ConversationParticipantEntity domain entity.
/// </summary>
public sealed class ConversationParticipantEntity : Entity
{
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid ConversationParticipantId { get; private set; } = Guid.NewGuid();
private ConversationParticipantEntity()
	{
	}

	/// <summary>Gets the persisted conversation id value.</summary>
	public Guid ConversationId { get; private set; }

	/// <summary>Gets the persisted user id value.</summary>
	public Guid UserId { get; private set; }

	/// <summary>Gets the persisted joined at value.</summary>
	public DateTimeOffset JoinedAt { get; private set; }

	/// <summary>Gets the persisted left at value.</summary>
	public DateTimeOffset? LeftAt { get; private set; }

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new ConversationParticipantEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static ConversationParticipantEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new ConversationParticipantEntity
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
