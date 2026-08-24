using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Models;

/// <summary>
/// Represents the GeneratedDocumentEntity domain entity.
/// </summary>
public sealed class GeneratedDocumentEntity : Entity
{
	/// <summary>Gets the persisted entity identifier.</summary>
	public Guid GeneratedDocumentId
	{
		get => Id;
		private set => Id = value;
	}

	private GeneratedDocumentEntity()
	{
	}

	/// <summary>Gets the persisted document template id value.</summary>
	public Guid DocumentTemplateId { get; private set; }

	/// <summary>Gets the persisted template version value.</summary>
	public int TemplateVersion { get; private set; }

	/// <summary>Gets the persisted student id value.</summary>
	public Guid? StudentId { get; private set; }

	/// <summary>Gets the persisted employee id value.</summary>
	public Guid? EmployeeId { get; private set; }

	/// <summary>Gets the persisted document number value.</summary>
	public string DocumentNumber { get; private set; } = string.Empty;

	/// <summary>Gets the persisted rendered content snapshot value.</summary>
	public string RenderedContentSnapshot { get; private set; } = string.Empty;

	/// <summary>Gets the persisted file url value.</summary>
	public string? FileUrl { get; private set; }

	/// <summary>Gets the persisted verification code value.</summary>
	public string? VerificationCode { get; private set; }

	/// <summary>Gets the persisted issued by value.</summary>
	public Guid? IssuedBy { get; private set; }

	/// <summary>Gets the persisted approved by value.</summary>
	public Guid? ApprovedBy { get; private set; }

	/// <summary>Gets the persisted issued at value.</summary>
	public DateTimeOffset? IssuedAt { get; private set; }

	/// <summary>Gets the persisted status value.</summary>
	public string Status { get; private set; } = string.Empty;

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new GeneratedDocumentEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static GeneratedDocumentEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new GeneratedDocumentEntity
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
