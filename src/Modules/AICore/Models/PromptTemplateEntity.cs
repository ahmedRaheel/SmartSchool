using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AICore.Models;

/// <summary>
/// Represents the PromptTemplateEntity domain entity.
/// </summary>
public sealed class PromptTemplateEntity : Entity
{
	private PromptTemplateEntity()
	{
	}

	/// <summary>Gets the persisted assistant type value.</summary>
	public string AssistantType { get; private set; } = string.Empty;

	/// <summary>Gets the persisted prompt type value.</summary>
	public string PromptType { get; private set; } = string.Empty;

	/// <summary>Gets the persisted prompt text value.</summary>
	public string PromptText { get; private set; } = string.Empty;

	/// <summary>Gets the persisted version value.</summary>
	public int Version { get; private set; }

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new PromptTemplateEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static PromptTemplateEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new PromptTemplateEntity
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
