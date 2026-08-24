using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Models;

/// <summary>
/// Represents the TopicPerformanceInsightEntity domain entity.
/// </summary>
public sealed class TopicPerformanceInsightEntity : Entity
{
	private TopicPerformanceInsightEntity()
	{
	}

	/// <summary>Gets the persisted class performance insight id value.</summary>
	public Guid ClassPerformanceInsightId { get; private set; }

	/// <summary>Gets the persisted subject id value.</summary>
	public Guid SubjectId { get; private set; }

	/// <summary>Gets the persisted topic value.</summary>
	public string Topic { get; private set; } = string.Empty;

	/// <summary>Gets the persisted average mastery score value.</summary>
	public decimal? AverageMasteryScore { get; private set; }

	/// <summary>Gets the persisted students struggling count value.</summary>
	public int StudentsStrugglingCount { get; private set; }

	/// <summary>Gets the persisted students mastered count value.</summary>
	public int StudentsMasteredCount { get; private set; }

	/// <summary>Gets the persisted risk level value.</summary>
	public string? RiskLevel { get; private set; }

	/// <summary>Gets the persisted recommended focus value.</summary>
	public string? RecommendedFocus { get; private set; }

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new TopicPerformanceInsightEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static TopicPerformanceInsightEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new TopicPerformanceInsightEntity
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
