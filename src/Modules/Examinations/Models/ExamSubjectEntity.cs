using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Examinations.Models;

/// <summary>
/// Represents the ExamSubjectEntity domain entity.
/// </summary>
public sealed class ExamSubjectEntity : Entity
{
	private ExamSubjectEntity()
	{
	}

	/// <summary>Gets the persisted exam id value.</summary>
	public Guid ExamId { get; private set; }

	/// <summary>Gets the persisted course offering id value.</summary>
	public Guid CourseOfferingId { get; private set; }

	/// <summary>Gets the persisted exam date value.</summary>
	public DateOnly? ExamDate { get; private set; }

	/// <summary>Gets the persisted start time value.</summary>
	public TimeOnly? StartTime { get; private set; }

	/// <summary>Gets the persisted duration minutes value.</summary>
	public int? DurationMinutes { get; private set; }

	/// <summary>Gets the persisted total marks value.</summary>
	public decimal TotalMarks { get; private set; }

	/// <summary>Gets the persisted passing marks value.</summary>
	public decimal? PassingMarks { get; private set; }

	/// <summary>Gets the persisted room id value.</summary>
	public Guid? RoomId { get; private set; }

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new ExamSubjectEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static ExamSubjectEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new ExamSubjectEntity
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
