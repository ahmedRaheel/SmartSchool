using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Models;

/// <summary>
/// Represents the ClassSectionEntity domain entity.
/// </summary>
public sealed class ClassSectionEntity : Entity
{
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid ClassSectionId { get; private set; } = Guid.NewGuid();

	private ClassSectionEntity()
	{
	}

	/// <summary>Gets the persisted campus id value.</summary>
	public Guid CampusId { get; private set; }

	/// <summary>Gets the persisted academic year id value.</summary>
	public Guid AcademicYearId { get; private set; }

	/// <summary>Gets the persisted program grade id value.</summary>
	public Guid? ProgramGradeId { get; private set; }

	/// <summary>Gets the class/grade level for this section.</summary>
	public Guid? GradeLevelId { get; private set; }

	/// <summary>Gets the persisted section id value.</summary>
	public Guid SectionId { get; private set; }

	/// <summary>Gets the persisted class teacher employee id value.</summary>
	public Guid? ClassTeacherEmployeeId { get; private set; }

	/// <summary>Gets the persisted room id value.</summary>
	public Guid? RoomId { get; private set; }

	/// <summary>Gets the human-readable room number assigned to this section.</summary>
	public string? RoomNo { get; private set; }

	/// <summary>Gets the persisted capacity value.</summary>
	public int? Capacity { get; private set; }

	/// <summary>Gets the persisted status value.</summary>
	public string Status { get; private set; } = string.Empty;

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new ClassSectionEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static ClassSectionEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new ClassSectionEntity
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
