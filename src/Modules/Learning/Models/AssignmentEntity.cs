using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Learning.Models;

/// <summary>
/// Represents the AssignmentEntity domain entity.
/// </summary>
public sealed class AssignmentEntity : Entity
{
	private AssignmentEntity()
	{
	}

	/// <summary>Gets the persisted course offering id value.</summary>
	public Guid CourseOfferingId { get; private set; }

	/// <summary>Gets the persisted class section id value.</summary>
	public Guid? ClassSectionId { get; private set; }

	/// <summary>Gets the persisted teaching group id value.</summary>
	public Guid? TeachingGroupId { get; private set; }

	/// <summary>Gets the persisted teacher employee id value.</summary>
	public Guid TeacherEmployeeId { get; private set; }

	/// <summary>Gets the persisted assignment type code value.</summary>
	public string AssignmentTypeCode { get; private set; } = string.Empty;

	/// <summary>Gets the persisted title value.</summary>
	public string Title { get; private set; } = string.Empty;

	/// <summary>Gets the persisted description value.</summary>
	public string? Description { get; private set; }

	/// <summary>Gets the persisted instructions value.</summary>
	public string? Instructions { get; private set; }

	/// <summary>Gets the persisted assigned at value.</summary>
	public DateTimeOffset AssignedAt { get; private set; }

	/// <summary>Gets the persisted due at value.</summary>
	public DateTimeOffset? DueAt { get; private set; }

	/// <summary>Gets the persisted total marks value.</summary>
	public decimal? TotalMarks { get; private set; }

	/// <summary>Gets the persisted allow late submission value.</summary>
	public bool AllowLateSubmission { get; private set; }

	/// <summary>Gets the persisted max attempts value.</summary>
	public int MaxAttempts { get; private set; }

	/// <summary>Gets the persisted status value.</summary>
	public string Status { get; private set; } = string.Empty;

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new AssignmentEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static AssignmentEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new AssignmentEntity
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
