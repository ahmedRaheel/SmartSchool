using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Models;

/// <summary>
/// Represents the StudentInterventionEntity domain entity.
/// </summary>
public sealed class StudentInterventionEntity : Entity
{
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid StudentInterventionId { get; private set; } = Guid.NewGuid();

	private StudentInterventionEntity()
	{
	}

	/// <summary>Gets the persisted student id value.</summary>
	public Guid StudentId { get; private set; }

	/// <summary>Gets the persisted subject id value.</summary>
	public Guid? SubjectId { get; private set; }

	/// <summary>Gets the persisted course offering id value.</summary>
	public Guid? CourseOfferingId { get; private set; }

	/// <summary>Gets the persisted teacher employee id value.</summary>
	public Guid? TeacherEmployeeId { get; private set; }

	/// <summary>Gets the persisted source prediction id value.</summary>
	public Guid? SourcePredictionId { get; private set; }

	/// <summary>Gets the persisted source recommendation id value.</summary>
	public Guid? SourceRecommendationId { get; private set; }

	/// <summary>Gets the persisted title value.</summary>
	public string Title { get; private set; } = string.Empty;

	/// <summary>Gets the persisted reason value.</summary>
	public string? Reason { get; private set; }

	/// <summary>Gets the persisted target outcome value.</summary>
	public string? TargetOutcome { get; private set; }

	/// <summary>Gets the persisted start date value.</summary>
	public DateOnly? StartDate { get; private set; }

	/// <summary>Gets the persisted target date value.</summary>
	public DateOnly? TargetDate { get; private set; }

	/// <summary>Gets the persisted status value.</summary>
	public string Status { get; private set; } = string.Empty;

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new StudentInterventionEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static StudentInterventionEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new StudentInterventionEntity
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
