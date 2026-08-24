using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Models;

/// <summary>
/// Represents the PredictionEvidenceEntity domain entity.
/// </summary>
public sealed class PredictionEvidenceEntity : Entity
{
<<<<<<< HEAD
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid PredictionEvidenceId { get; private set; } = Guid.NewGuid();
=======
	/// <summary>Gets the persisted entity identifier.</summary>
	public Guid PredictionEvidenceId
	{
		get => Id;
		private set => Id = value;
	}
>>>>>>> c40f31f829a59dcdb7fd9fe0046a26e6e366eca0

	private PredictionEvidenceEntity()
	{
	}

	/// <summary>Gets the persisted student performance prediction id value.</summary>
	public Guid StudentPerformancePredictionId { get; private set; }

	/// <summary>Gets the persisted evidence type value.</summary>
	public string EvidenceType { get; private set; } = string.Empty;

	/// <summary>Gets the persisted source entity type value.</summary>
	public string? SourceEntityType { get; private set; }

	/// <summary>Gets the persisted source entity id value.</summary>
	public Guid? SourceEntityId { get; private set; }

	/// <summary>Gets the persisted numeric value value.</summary>
	public decimal? NumericValue { get; private set; }

	/// <summary>Gets the persisted text value value.</summary>
	public string? TextValue { get; private set; }

	/// <summary>Gets the persisted normalized value value.</summary>
	public decimal? NormalizedValue { get; private set; }

	/// <summary>Gets the persisted weight value.</summary>
	public decimal? Weight { get; private set; }

	/// <summary>Gets the persisted occurred at value.</summary>
	public DateTimeOffset? OccurredAt { get; private set; }

	/// <summary>Gets the persisted explanation value.</summary>
	public string? Explanation { get; private set; }

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new PredictionEvidenceEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static PredictionEvidenceEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new PredictionEvidenceEntity
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
