using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Models;

/// <summary>
/// Represents the ClassPerformanceInsightEntity domain entity.
/// </summary>
public sealed class ClassPerformanceInsightEntity : Entity
{
	/// <summary>Gets the persisted entity identifier.</summary>
	public Guid ClassPerformanceInsightId
	{
		get => Id;
		private set => Id = value;
	}

	private ClassPerformanceInsightEntity()
	{
	}

	/// <summary>Gets the persisted academic year id value.</summary>
	public Guid AcademicYearId { get; private set; }

	/// <summary>Gets the persisted term id value.</summary>
	public Guid? TermId { get; private set; }

	/// <summary>Gets the persisted class section id value.</summary>
	public Guid ClassSectionId { get; private set; }

	/// <summary>Gets the persisted course offering id value.</summary>
	public Guid CourseOfferingId { get; private set; }

	/// <summary>Gets the persisted teacher employee id value.</summary>
	public Guid? TeacherEmployeeId { get; private set; }

	/// <summary>Gets the persisted students count value.</summary>
	public int StudentsCount { get; private set; }

	/// <summary>Gets the persisted on track count value.</summary>
	public int OnTrackCount { get; private set; }

	/// <summary>Gets the persisted needs attention count value.</summary>
	public int NeedsAttentionCount { get; private set; }

	/// <summary>Gets the persisted high risk count value.</summary>
	public int HighRiskCount { get; private set; }

	/// <summary>Gets the persisted predicted class average value.</summary>
	public decimal? PredictedClassAverage { get; private set; }

	/// <summary>Gets the persisted current class average value.</summary>
	public decimal? CurrentClassAverage { get; private set; }

	/// <summary>Gets the persisted trend value.</summary>
	public string? Trend { get; private set; }

	/// <summary>Gets the persisted summary value.</summary>
	public string? Summary { get; private set; }

	/// <summary>Gets the persisted generated at value.</summary>
	public DateTimeOffset GeneratedAt { get; private set; }

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new ClassPerformanceInsightEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static ClassPerformanceInsightEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new ClassPerformanceInsightEntity
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
