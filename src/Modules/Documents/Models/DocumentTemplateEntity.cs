using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Models;

/// <summary>
/// Represents the DocumentTemplateEntity domain entity.
/// </summary>
public sealed class DocumentTemplateEntity : Entity
{
<<<<<<< HEAD
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid DocumentTemplateId { get; private set; } = Guid.NewGuid();
=======
	/// <summary>Gets the persisted entity identifier.</summary>
	public Guid DocumentTemplateId
	{
		get => Id;
		private set => Id = value;
	}
>>>>>>> c40f31f829a59dcdb7fd9fe0046a26e6e366eca0

	private DocumentTemplateEntity()
	{
	}

	/// <summary>Gets the persisted campus id value.</summary>
	public Guid? CampusId { get; private set; }

	/// <summary>Gets the persisted academic system id value.</summary>
	public Guid? AcademicSystemId { get; private set; }

	/// <summary>Gets the persisted document type code value.</summary>
	public string DocumentTypeCode { get; private set; } = string.Empty;

	/// <summary>Gets the persisted subject template value.</summary>
	public string? SubjectTemplate { get; private set; }

	/// <summary>Gets the persisted header html value.</summary>
	public string? HeaderHtml { get; private set; }

	/// <summary>Gets the persisted body html value.</summary>
	public string BodyHtml { get; private set; } = string.Empty;

	/// <summary>Gets the persisted footer html value.</summary>
	public string? FooterHtml { get; private set; }

	/// <summary>Gets the persisted language code value.</summary>
	public string LanguageCode { get; private set; } = string.Empty;

	/// <summary>Gets the persisted version value.</summary>
	public int Version { get; private set; }

	/// <summary>Gets the persisted requires approval value.</summary>
	public bool RequiresApproval { get; private set; }

	/// <summary>Gets the business code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets optional domain metadata serialized as JSON.</summary>
	public string? MetadataJson { get; private set; }

	/// <summary>Creates a new DocumentTemplateEntity.</summary>
	/// <param name="tenantId">The owning tenant identifier.</param>
	/// <param name="code">The business code.</param>
	/// <param name="name">The display name.</param>
	/// <param name="metadataJson">Optional domain metadata.</param>
	/// <returns>The newly created entity.</returns>
	public static DocumentTemplateEntity Create(
		Guid tenantId,
		string code,
		string name,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);

		return new DocumentTemplateEntity
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
