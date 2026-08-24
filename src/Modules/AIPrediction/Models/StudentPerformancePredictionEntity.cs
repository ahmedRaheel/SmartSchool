using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Models;

/// <summary>
/// Represents the StudentPerformancePredictionEntity domain entity.
/// </summary>
public sealed class StudentPerformancePredictionEntity : Entity
{
	private StudentPerformancePredictionEntity()
	{
	}

	/// <summary>Gets the persisted student id value.</summary>
	public Guid StudentId { get; private set; }

	/// <summary>Gets the persisted academic year id value.</summary>
	public Guid AcademicYearId { get; private set; }

	/// <summary>Gets the persisted term id value.</summary>
	public Guid? TermId { get; private set; }

	/// <summary>Gets the persisted course offering id value.</summary>
	public Guid CourseOfferingId { get; private set; }

	/// <summary>Gets the persisted subject id value.</summary>
	public Guid SubjectId { get; private set; }

	/// <summary>Gets the persisted target exam id value.</summary>
	public Guid? TargetExamId { get; private set; }

	/// <summary>Gets the persisted target exam subject id value.</summary>
	public Guid? TargetExamSubjectId { get; private set; }

	/// <summary>Gets the persisted target exam type code value.</summary>
	public string? TargetExamTypeCode { get; private set; }

	/// <summary>Gets the persisted target date value.</summary>
	public DateOnly? TargetDate { get; private set; }

	/// <summary>Gets the persisted predicted marks value.</summary>
	public decimal? PredictedMarks { get; private set; }

	/// <summary>Gets the persisted predicted percentage value.</summary>
	public decimal? PredictedPercentage { get; private set; }

	/// <summary>Gets the persisted predicted grade value.</summary>
	public string? PredictedGrade { get; private set; }

	/// <summary>Gets the persisted lower bound percentage value.</summary>
	public decimal? LowerBoundPercentage { get; private set; }

	/// <summary>Gets the persisted upper bound percentage value.</summary>
	public decimal? UpperBoundPercentage { get; private set; }

	/// <summary>Gets the persisted confidence score value.</summary>
	public decimal? ConfidenceScore { get; private set; }

	/// <summary>Gets the persisted pass probability value.</summary>
	public decimal? PassProbability { get; private set; }

	/// <summary>Gets the persisted fail probability value.</summary>
	public decimal? FailProbability { get; private set; }

	/// <summary>Gets the persisted target grade value.</summary>
	public string? TargetGrade { get; private set; }

	/// <summary>Gets the persisted target grade probability value.</summary>
	public decimal? TargetGradeProbability { get; private set; }

	/// <summary>Gets the persisted trend value.</summary>
	public string? Trend { get; private set; }

	/// <summary>Gets the persisted risk level value.</summary>
	public string? RiskLevel { get; private set; }

	/// <summary>Gets the persisted explanation summary value.</summary>
	public string? ExplanationSummary { get; private set; }

	/// <summary>Gets the persisted explanation value.</summary>
	public string? Explanation { get; private set; }

	/// <summary>Gets the persisted prediction model id value.</summary>
	public Guid? PredictionModelId { get; private set; }

	/// <summary>Gets the persisted model version value.</summary>
	public string? ModelVersion { get; private set; }

	/// <summary>Gets the persisted generated at value.</summary>
	public DateTimeOffset GeneratedAt { get; private set; }

	/// <summary>Gets the persisted expires at value.</summary>
	public DateTimeOffset? ExpiresAt { get; private set; }

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new StudentPerformancePredictionEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static StudentPerformancePredictionEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new StudentPerformancePredictionEntity
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
