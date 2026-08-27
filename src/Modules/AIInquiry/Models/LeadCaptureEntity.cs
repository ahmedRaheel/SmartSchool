using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIInquiry.Models;

/// <summary>
/// Represents the LeadCaptureEntity domain entity.
/// </summary>
public sealed class LeadCaptureEntity : Entity
{
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid LeadCaptureId { get; private set; } = Guid.NewGuid();

	private LeadCaptureEntity()
	{
	}

	/// <summary>Gets the persisted inquiry conversation id value.</summary>
	public Guid InquiryConversationId { get; private set; }

	/// <summary>Gets the persisted phone value.</summary>
	public string? Phone { get; private set; }

	/// <summary>Gets the persisted email value.</summary>
	public string? Email { get; private set; }

	/// <summary>Gets the persisted interested campus id value.</summary>
	public Guid? InterestedCampusId { get; private set; }

	/// <summary>Gets the persisted interested program id value.</summary>
	public Guid? InterestedProgramId { get; private set; }

	/// <summary>Gets the persisted interested grade id value.</summary>
	public Guid? InterestedGradeId { get; private set; }

	/// <summary>Gets the persisted notes value.</summary>
	public string? Notes { get; private set; }

	/// <summary>Gets the persisted captured at value.</summary>
	public DateTimeOffset CapturedAt { get; private set; }

	/// <summary>Gets the persisted converted inquiry id value.</summary>
	public Guid? ConvertedInquiryId { get; private set; }

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new LeadCaptureEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static LeadCaptureEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new LeadCaptureEntity
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
