using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Models;

/// <summary>
/// Represents the MessageReceiptEntity domain entity.
/// </summary>
public sealed class MessageReceiptEntity : Entity
{
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid MessageReceiptId { get; private set; } = Guid.NewGuid();
private MessageReceiptEntity()
	{
	}

	/// <summary>Gets the persisted message id value.</summary>
	public Guid MessageId { get; private set; }

	/// <summary>Gets the persisted user id value.</summary>
	public Guid UserId { get; private set; }

	/// <summary>Gets the persisted delivered at value.</summary>
	public DateTimeOffset? DeliveredAt { get; private set; }

	/// <summary>Gets the persisted read at value.</summary>
	public DateTimeOffset? ReadAt { get; private set; }

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new MessageReceiptEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static MessageReceiptEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new MessageReceiptEntity
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
