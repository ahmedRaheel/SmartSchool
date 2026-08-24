using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIInquiry.Models;

/// <summary>
/// Represents the InquiryConversationEntity domain entity.
/// </summary>
public sealed class InquiryConversationEntity : Entity
{
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid InquiryConversationId { get; private set; } = Guid.NewGuid();

	private InquiryConversationEntity()
	{
	}

	/// <summary>Gets the persisted campus id value.</summary>
	public Guid? CampusId { get; private set; }

	/// <summary>Gets the persisted visitor session id value.</summary>
	public string VisitorSessionId { get; private set; } = string.Empty;

	/// <summary>Gets the persisted user id value.</summary>
	public Guid? UserId { get; private set; }

	/// <summary>Gets the persisted visitor name value.</summary>
	public string? VisitorName { get; private set; }

	/// <summary>Gets the persisted phone value.</summary>
	public string? Phone { get; private set; }

	/// <summary>Gets the persisted email value.</summary>
	public string? Email { get; private set; }

	/// <summary>Gets the persisted interested program id value.</summary>
	public Guid? InterestedProgramId { get; private set; }

	/// <summary>Gets the persisted started at value.</summary>
	public DateTimeOffset StartedAt { get; private set; }

	/// <summary>Gets the persisted ended at value.</summary>
	public DateTimeOffset? EndedAt { get; private set; }

	/// <summary>Gets the persisted status value.</summary>
	public string Status { get; private set; } = string.Empty;

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new InquiryConversationEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static InquiryConversationEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new InquiryConversationEntity
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
