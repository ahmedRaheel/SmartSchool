using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Models;

/// <summary>
/// Represents the InterviewEntity domain entity.
/// </summary>
public sealed class InterviewEntity : Entity
{
<<<<<<< HEAD
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid InterviewId { get; private set; } = Guid.NewGuid();
=======
	/// <summary>Gets the persisted entity identifier.</summary>
	public Guid InterviewId
	{
		get => Id;
		private set => Id = value;
	}
>>>>>>> c40f31f829a59dcdb7fd9fe0046a26e6e366eca0

	private InterviewEntity()
	{
	}

	/// <summary>Gets the persisted job application id value.</summary>
	public Guid JobApplicationId { get; private set; }

	/// <summary>Gets the persisted interview type code value.</summary>
	public string InterviewTypeCode { get; private set; } = string.Empty;

	/// <summary>Gets the persisted round number value.</summary>
	public int RoundNumber { get; private set; }

	/// <summary>Gets the persisted scheduled at value.</summary>
	public DateTimeOffset? ScheduledAt { get; private set; }

	/// <summary>Gets the persisted duration minutes value.</summary>
	public int? DurationMinutes { get; private set; }

	/// <summary>Gets the persisted location value.</summary>
	public string? Location { get; private set; }

	/// <summary>Gets the persisted meeting url value.</summary>
	public string? MeetingUrl { get; private set; }

	/// <summary>Gets the persisted status value.</summary>
	public string Status { get; private set; } = string.Empty;

	/// <summary>Gets the persisted overall score value.</summary>
	public decimal? OverallScore { get; private set; }

	/// <summary>Gets the persisted recommendation value.</summary>
	public string? Recommendation { get; private set; }

	/// <summary>Gets the persisted notes value.</summary>
	public string? Notes { get; private set; }

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new InterviewEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static InterviewEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new InterviewEntity
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
