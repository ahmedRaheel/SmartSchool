using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Learning.Models;

/// <summary>
/// Represents the AssignmentSubmissionEntity domain entity.
/// </summary>
public sealed class AssignmentSubmissionEntity : Entity
{
	private AssignmentSubmissionEntity()
	{
	}

	/// <summary>Gets the persisted submission id value.</summary>
	public Guid SubmissionId { get; private set; }

	/// <summary>Gets the persisted academic assignment id value.</summary>
	public Guid AcademicAssignmentId { get; private set; }

	/// <summary>Gets the persisted student id value.</summary>
	public Guid StudentId { get; private set; }

	/// <summary>Gets the persisted attempt no value.</summary>
	public int AttemptNo { get; private set; }

	/// <summary>Gets the persisted submitted at value.</summary>
	public DateTimeOffset? SubmittedAt { get; private set; }

	/// <summary>Gets the persisted submission text value.</summary>
	public string? SubmissionText { get; private set; }

	/// <summary>Gets the persisted marks obtained value.</summary>
	public decimal? MarksObtained { get; private set; }

	/// <summary>Gets the persisted teacher feedback value.</summary>
	public string? TeacherFeedback { get; private set; }

	/// <summary>Gets the persisted status value.</summary>
	public string Status { get; private set; } = string.Empty;

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new AssignmentSubmissionEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static AssignmentSubmissionEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new AssignmentSubmissionEntity
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
