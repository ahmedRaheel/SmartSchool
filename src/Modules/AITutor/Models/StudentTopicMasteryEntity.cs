using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AITutor.Models;

/// <summary>
/// Represents the StudentTopicMasteryEntity domain entity.
/// </summary>
public sealed class StudentTopicMasteryEntity : Entity
{
<<<<<<< HEAD
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid StudentTopicMasteryId { get; private set; } = Guid.NewGuid();
=======
	/// <summary>Gets the persisted entity identifier.</summary>
	public Guid StudentTopicMasteryId
	{
		get => Id;
		private set => Id = value;
	}
>>>>>>> c40f31f829a59dcdb7fd9fe0046a26e6e366eca0

	private StudentTopicMasteryEntity()
	{
	}

	/// <summary>Gets the persisted student id value.</summary>
	public Guid StudentId { get; private set; }

	/// <summary>Gets the persisted subject id value.</summary>
	public Guid SubjectId { get; private set; }

	/// <summary>Gets the persisted topic value.</summary>
	public string Topic { get; private set; } = string.Empty;

	/// <summary>Gets the persisted mastery score value.</summary>
	public decimal? MasteryScore { get; private set; }

	/// <summary>Gets the persisted confidence score value.</summary>
	public decimal? ConfidenceScore { get; private set; }

	/// <summary>Gets the persisted evidence count value.</summary>
	public int EvidenceCount { get; private set; }

	/// <summary>Gets the persisted last assessed at value.</summary>
	public DateTimeOffset? LastAssessedAt { get; private set; }

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new StudentTopicMasteryEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static StudentTopicMasteryEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new StudentTopicMasteryEntity
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
