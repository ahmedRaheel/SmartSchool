using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Activities.Models;

/// <summary>
/// Represents the AwardEntity domain entity.
/// </summary>
public sealed class AwardEntity : Entity
{
	/// <summary>Gets the persisted entity identifier.</summary>
	public Guid StudentAwardId
	{
		get => Id;
		private set => Id = value;
	}

	private AwardEntity()
	{
	}

	/// <summary>Gets the persisted student id value.</summary>
	public Guid StudentId { get; private set; }

	/// <summary>Gets the persisted award type code value.</summary>
	public string AwardTypeCode { get; private set; } = string.Empty;

	/// <summary>Gets the persisted title value.</summary>
	public string Title { get; private set; } = string.Empty;

	/// <summary>Gets the persisted description value.</summary>
	public string? Description { get; private set; }

	/// <summary>Gets the persisted award date value.</summary>
	public DateOnly AwardDate { get; private set; }

	/// <summary>Gets the persisted approved by value.</summary>
	public Guid? ApprovedBy { get; private set; }

	/// <summary>Gets the persisted generated document id value.</summary>
	public Guid? GeneratedDocumentId { get; private set; }

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new AwardEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static AwardEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new AwardEntity
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
