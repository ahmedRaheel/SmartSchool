using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AITutor.Models;

/// <summary>
/// Represents the QuizAttemptEntity domain entity.
/// </summary>
public sealed class QuizAttemptEntity : Entity
{
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid StudentQuizAttemptId { get; private set; } = Guid.NewGuid();

	private QuizAttemptEntity()
	{
	}

	/// <summary>Gets the persisted generated quiz id value.</summary>
	public Guid GeneratedQuizId { get; private set; }

	/// <summary>Gets the persisted student id value.</summary>
	public Guid StudentId { get; private set; }

	/// <summary>Gets the persisted started at value.</summary>
	public DateTimeOffset StartedAt { get; private set; }

	/// <summary>Gets the persisted completed at value.</summary>
	public DateTimeOffset? CompletedAt { get; private set; }

	/// <summary>Gets the persisted score value.</summary>
	public decimal? Score { get; private set; }

	/// <summary>Gets the persisted answers value.</summary>
	public string? Answers { get; private set; }

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new QuizAttemptEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static QuizAttemptEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new QuizAttemptEntity
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
