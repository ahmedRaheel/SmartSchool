using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Models;

/// <summary>
/// Represents the TimetableEntryEntity domain entity.
/// </summary>
public sealed class TimetableEntryEntity : Entity
{
	/// <summary>Gets the persisted entity identifier.</summary>
	public Guid TimetableEntryId
	{
		get => Id;
		private set => Id = value;
	}

	private TimetableEntryEntity()
	{
	}

	/// <summary>Gets the persisted timetable id value.</summary>
	public Guid TimetableId { get; private set; }

	/// <summary>Gets the persisted day of week value.</summary>
	public int DayOfWeek { get; private set; }

	/// <summary>Gets the persisted timetable period id value.</summary>
	public Guid TimetablePeriodId { get; private set; }

	/// <summary>Gets the persisted class section id value.</summary>
	public Guid? ClassSectionId { get; private set; }

	/// <summary>Gets the persisted teaching group id value.</summary>
	public Guid? TeachingGroupId { get; private set; }

	/// <summary>Gets the persisted course offering id value.</summary>
	public Guid? CourseOfferingId { get; private set; }

	/// <summary>Gets the persisted teacher course assignment id value.</summary>
	public Guid? TeacherCourseAssignmentId { get; private set; }

	/// <summary>Gets the persisted room id value.</summary>
	public Guid? RoomId { get; private set; }

	/// <summary>Gets the persisted entry type value.</summary>
	public string EntryType { get; private set; } = string.Empty;

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new TimetableEntryEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static TimetableEntryEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new TimetableEntryEntity
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
