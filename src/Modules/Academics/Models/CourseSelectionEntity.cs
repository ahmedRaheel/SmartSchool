using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Models;

/// <summary>
/// Represents the CourseSelectionEntity domain entity.
/// </summary>
public sealed class CourseSelectionEntity : Entity
{
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid StudentCourseEnrollmentId { get; private set; } = Guid.NewGuid();

	private CourseSelectionEntity()
	{
	}

	/// <summary>Gets the persisted student enrollment id value.</summary>
	public Guid StudentEnrollmentId { get; private set; }

	/// <summary>Gets the persisted course offering id value.</summary>
	public Guid CourseOfferingId { get; private set; }

	/// <summary>Gets the persisted enrollment type code value.</summary>
	public string EnrollmentTypeCode { get; private set; } = string.Empty;

	/// <summary>Gets the persisted selected at value.</summary>
	public DateTimeOffset SelectedAt { get; private set; }

	/// <summary>Gets the persisted approved by value.</summary>
	public Guid? ApprovedBy { get; private set; }

	/// <summary>Gets the persisted approved at value.</summary>
	public DateTimeOffset? ApprovedAt { get; private set; }

	/// <summary>Gets the persisted status value.</summary>
	public string Status { get; private set; } = string.Empty;

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new CourseSelectionEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static CourseSelectionEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new CourseSelectionEntity
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
