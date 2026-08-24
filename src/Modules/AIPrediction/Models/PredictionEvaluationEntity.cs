using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Models;

/// <summary>
/// Represents the PredictionEvaluationEntity domain entity.
/// </summary>
public sealed class PredictionEvaluationEntity : Entity
{
	private PredictionEvaluationEntity()
	{
	}

	/// <summary>Gets the persisted student performance prediction id value.</summary>
	public Guid StudentPerformancePredictionId { get; private set; }

	/// <summary>Gets the persisted student exam result id value.</summary>
	public Guid StudentExamResultId { get; private set; }

	/// <summary>Gets the persisted predicted percentage value.</summary>
	public decimal? PredictedPercentage { get; private set; }

	/// <summary>Gets the persisted actual percentage value.</summary>
	public decimal? ActualPercentage { get; private set; }

	/// <summary>Gets the persisted absolute error value.</summary>
	public decimal? AbsoluteError { get; private set; }

	/// <summary>Gets the persisted predicted grade value.</summary>
	public string? PredictedGrade { get; private set; }

	/// <summary>Gets the persisted actual grade value.</summary>
	public string? ActualGrade { get; private set; }

	/// <summary>Gets the persisted grade correct value.</summary>
	public bool? GradeCorrect { get; private set; }

	/// <summary>Gets the persisted evaluated at value.</summary>
	public DateTimeOffset EvaluatedAt { get; private set; }

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new PredictionEvaluationEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static PredictionEvaluationEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new PredictionEvaluationEntity
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
