using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIInquiry.Models;

/// <summary>
/// Represents the HumanHandoffEntity domain entity.
/// </summary>
public sealed class HumanHandoffEntity : Entity
{
	private HumanHandoffEntity()
	{
	}

	/// <summary>Gets the persisted inquiry conversation id value.</summary>
	public Guid InquiryConversationId { get; private set; }

	/// <summary>Gets the persisted requested at value.</summary>
	public DateTimeOffset RequestedAt { get; private set; }

	/// <summary>Gets the persisted reason value.</summary>
	public string? Reason { get; private set; }

	/// <summary>Gets the persisted assigned to user id value.</summary>
	public Guid? AssignedToUserId { get; private set; }

	/// <summary>Gets the persisted accepted at value.</summary>
	public DateTimeOffset? AcceptedAt { get; private set; }

	/// <summary>Gets the persisted resolved at value.</summary>
	public DateTimeOffset? ResolvedAt { get; private set; }

	/// <summary>Gets the persisted status value.</summary>
	public string Status { get; private set; } = string.Empty;

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new HumanHandoffEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static HumanHandoffEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new HumanHandoffEntity
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
