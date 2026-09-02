using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Models;

/// <summary>
/// Represents the CourseOfferingEntity domain entity.
/// </summary>
public sealed class CourseOfferingEntity : Entity
{
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid CourseOfferingId { get; private set; } = Guid.NewGuid();

	public Guid BranchId { get; private set; }

	private CourseOfferingEntity()
	{
	}

	/// <summary>Gets the persisted campus id value.</summary>
	public Guid CampusId { get; private set; }

	/// <summary>Gets the persisted academic year id value.</summary>
	public Guid AcademicYearId { get; private set; }

	/// <summary>Gets the persisted term id value.</summary>
	public Guid? TermId { get; private set; }

	/// <summary>Gets the persisted program subject id value.</summary>
	public Guid ProgramSubjectId { get; private set; }

	/// <summary>Gets the persisted display name value.</summary>
	public string? DisplayName { get; private set; }

	/// <summary>Gets the persisted status value.</summary>
	public string Status { get; private set; } = string.Empty;

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new CourseOfferingEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static CourseOfferingEntity Create(
		Guid tenantId,
        Guid branchId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new CourseOfferingEntity
		{
			TenantId = tenantId,
            BranchId = branchId,
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
