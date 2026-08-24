using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AITutor.Models;

/// <summary>
/// Represents the TutorSessionEntity domain entity.
/// </summary>
public sealed class TutorSessionEntity : Entity
{
	private TutorSessionEntity()
	{
	}

	/// <summary>Gets the persisted tutor conversation id value.</summary>
	public Guid TutorConversationId { get; private set; }

	/// <summary>Gets the persisted topic value.</summary>
	public string? Topic { get; private set; }

	/// <summary>Gets the persisted learning objective value.</summary>
	public string? LearningObjective { get; private set; }

	/// <summary>Gets the persisted started at value.</summary>
	public DateTimeOffset StartedAt { get; private set; }

	/// <summary>Gets the persisted ended at value.</summary>
	public DateTimeOffset? EndedAt { get; private set; }

	/// <summary>Gets the persisted session summary value.</summary>
	public string? SessionSummary { get; private set; }

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new TutorSessionEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static TutorSessionEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new TutorSessionEntity
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
