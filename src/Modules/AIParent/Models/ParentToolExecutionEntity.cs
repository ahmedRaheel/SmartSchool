using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIParent.Models;

/// <summary>
/// Represents the ParentToolExecutionEntity domain entity.
/// </summary>
public sealed class ParentToolExecutionEntity : Entity
{
	private ParentToolExecutionEntity()
	{
	}

	/// <summary>Gets the persisted parent conversation id value.</summary>
	public Guid ParentConversationId { get; private set; }

	/// <summary>Gets the persisted tool definition id value.</summary>
	public Guid ToolDefinitionId { get; private set; }

	/// <summary>Gets the persisted student id value.</summary>
	public Guid? StudentId { get; private set; }

	/// <summary>Gets the persisted input payload value.</summary>
	public string? InputPayload { get; private set; }

	/// <summary>Gets the persisted output payload value.</summary>
	public string? OutputPayload { get; private set; }

	/// <summary>Gets the persisted status value.</summary>
	public string Status { get; private set; } = string.Empty;

	/// <summary>Gets the persisted executed at value.</summary>
	public DateTimeOffset ExecutedAt { get; private set; }

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new ParentToolExecutionEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static ParentToolExecutionEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new ParentToolExecutionEntity
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
