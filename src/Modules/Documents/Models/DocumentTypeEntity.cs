using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Models;

/// <summary>
/// Defines a normalized classification of uploaded documents.
/// </summary>
public sealed class DocumentTypeEntity : Entity
{
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid DocumentTypeId { get; private set; } = Guid.NewGuid();
private DocumentTypeEntity()
	{
	}

	/// <summary>Gets the stable document type code.</summary>
	public string Code { get; private set; } = string.Empty;

	/// <summary>Gets the display name.</summary>
	public string Name { get; private set; } = string.Empty;

	/// <summary>Gets the owner category such as Student, Parent, Teacher, Driver or Any.</summary>
	public string OwnerCategory { get; private set; } = string.Empty;

	/// <summary>Gets whether the type may contain identity-sensitive data.</summary>
	public bool IsIdentityDocument { get; private set; }

	/// <summary>Gets whether an expiry date is normally expected.</summary>
	public bool RequiresExpiryDate { get; private set; }

	/// <summary>Gets whether verification is required before the document is accepted.</summary>
	public bool RequiresVerification { get; private set; }

	/// <summary>Creates a document type.</summary>
	public static DocumentTypeEntity Create(
		Guid tenantId,
		string code,
		string name,
		string ownerCategory,
		bool isIdentityDocument,
		bool requiresExpiryDate,
		bool requiresVerification)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);
		ArgumentException.ThrowIfNullOrWhiteSpace(ownerCategory);

		return new DocumentTypeEntity
		{
			TenantId = tenantId,
			Code = code.Trim().ToUpperInvariant(),
			Name = name.Trim(),
			OwnerCategory = ownerCategory.Trim(),
			IsIdentityDocument = isIdentityDocument,
			RequiresExpiryDate = requiresExpiryDate,
			RequiresVerification = requiresVerification
		};
	}
}
