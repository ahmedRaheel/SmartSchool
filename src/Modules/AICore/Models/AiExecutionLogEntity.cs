using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AICore.Models;

/// <summary>
/// Represents the AiExecutionLogEntity domain entity.
/// </summary>
public sealed class AiExecutionLogEntity : Entity
{
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid AiExecutionLogId { get; private set; } = Guid.NewGuid();

	private AiExecutionLogEntity()
	{
	}

	/// <summary>Gets the persisted assistant type value.</summary>
	public string AssistantType { get; private set; } = string.Empty;

	/// <summary>Gets the persisted conversation reference id value.</summary>
	public Guid? ConversationReferenceId { get; private set; }

	/// <summary>Gets the persisted user id value.</summary>
	public Guid? UserId { get; private set; }

	/// <summary>Gets the persisted model configuration id value.</summary>
	public Guid? ModelConfigurationId { get; private set; }

	/// <summary>Gets the persisted prompt tokens value.</summary>
	public int? PromptTokens { get; private set; }

	/// <summary>Gets the persisted completion tokens value.</summary>
	public int? CompletionTokens { get; private set; }

	/// <summary>Gets the persisted total tokens value.</summary>
	public int? TotalTokens { get; private set; }

	/// <summary>Gets the persisted estimated cost value.</summary>
	public decimal? EstimatedCost { get; private set; }

	/// <summary>Gets the persisted latency ms value.</summary>
	public int? LatencyMs { get; private set; }

	/// <summary>Gets the persisted status value.</summary>
	public string Status { get; private set; } = string.Empty;

	/// <summary>Gets the persisted correlation id value.</summary>
	public string? CorrelationId { get; private set; }

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new AiExecutionLogEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static AiExecutionLogEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new AiExecutionLogEntity
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
