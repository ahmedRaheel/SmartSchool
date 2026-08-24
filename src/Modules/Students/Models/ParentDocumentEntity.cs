using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Documents;

namespace SmartSchool.Modules.Students.Models;

/// <summary>
/// Stores metadata for a document attached to a parent/guardian.
/// File bytes are stored in the configured object/file storage provider.
/// </summary>
public sealed class ParentDocumentEntity : Entity
{
<<<<<<< HEAD
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid ParentDocumentId { get; private set; } = Guid.NewGuid();
private ParentDocumentEntity()
=======
	/// <summary>Gets the persisted entity identifier.</summary>
	public Guid Id
	{
		get => Id;
		private set => Id = value;
	}

	private ParentDocumentEntity()
>>>>>>> c40f31f829a59dcdb7fd9fe0046a26e6e366eca0
	{
	}

	/// <summary>Gets the owning parent/guardian identifier.</summary>
	public Guid ParentId { get; private set; }

	/// <summary>Gets the normalized document type identifier.</summary>
	public Guid DocumentTypeId { get; private set; }

	/// <summary>Gets the original client file name.</summary>
	public string OriginalFileName { get; private set; } = string.Empty;

	/// <summary>Gets the media/MIME type.</summary>
	public string ContentType { get; private set; } = string.Empty;

	/// <summary>Gets the file size in bytes.</summary>
	public long FileSizeBytes { get; private set; }

	/// <summary>Gets the storage provider code, for example Local, S3 or AzureBlob.</summary>
	public string StorageProvider { get; private set; } = string.Empty;

	/// <summary>Gets the provider-specific object key. This is not a public URL.</summary>
	public string StorageKey { get; private set; } = string.Empty;

	/// <summary>Gets the SHA-256 checksum used for integrity and duplicate detection.</summary>
	public string Sha256Hash { get; private set; } = string.Empty;

	/// <summary>Gets an optional certificate, CNIC, passport or license number.</summary>
	public string? DocumentNumber { get; private set; }

	/// <summary>Gets the issue date when applicable.</summary>
	public DateOnly? IssuedOn { get; private set; }

	/// <summary>Gets the expiry date when applicable.</summary>
	public DateOnly? ExpiresOn { get; private set; }

	/// <summary>Gets whether an authorized user verified the document.</summary>
	public bool IsVerified { get; private set; }

	/// <summary>Gets the user who verified the document.</summary>
	public Guid? VerifiedByUserId { get; private set; }

	/// <summary>Gets the UTC verification timestamp.</summary>
	public DateTimeOffset? VerifiedAt { get; private set; }

	/// <summary>Gets optional administrative notes.</summary>
	public string? Notes { get; private set; }

	/// <summary>Creates document metadata for a parent/guardian.</summary>
	public static ParentDocumentEntity Create(
		Guid tenantId,
		Guid parentId,
		DocumentMetadata metadata)
	{
		ArgumentNullException.ThrowIfNull(metadata);
		ValidateMetadata(metadata);

		return new ParentDocumentEntity
		{
			TenantId = tenantId,
			ParentId = parentId,
			DocumentTypeId = metadata.DocumentTypeId,
			OriginalFileName = metadata.OriginalFileName.Trim(),
			ContentType = metadata.ContentType.Trim(),
			FileSizeBytes = metadata.FileSizeBytes,
			StorageProvider = metadata.StorageProvider.Trim(),
			StorageKey = metadata.StorageKey.Trim(),
			Sha256Hash = metadata.Sha256Hash.Trim().ToUpperInvariant(),
			DocumentNumber = metadata.DocumentNumber?.Trim(),
			IssuedOn = metadata.IssuedOn,
			ExpiresOn = metadata.ExpiresOn,
			Notes = metadata.Notes?.Trim()
		};
	}

	/// <summary>Marks the document as verified by an authorized user.</summary>
	public void Verify(Guid verifiedByUserId)
	{
		IsVerified = true;
		VerifiedByUserId = verifiedByUserId;
		VerifiedAt = DateTimeOffset.UtcNow;
		MarkAsUpdated();
	}

	/// <summary>Clears the current verification state.</summary>
	public void RevokeVerification()
	{
		IsVerified = false;
		VerifiedByUserId = null;
		VerifiedAt = null;
		MarkAsUpdated();
	}

	private static void ValidateMetadata(DocumentMetadata metadata)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(metadata.OriginalFileName);
		ArgumentException.ThrowIfNullOrWhiteSpace(metadata.ContentType);
		ArgumentException.ThrowIfNullOrWhiteSpace(metadata.StorageProvider);
		ArgumentException.ThrowIfNullOrWhiteSpace(metadata.StorageKey);
		ArgumentException.ThrowIfNullOrWhiteSpace(metadata.Sha256Hash);

		if (metadata.DocumentTypeId == Guid.Empty)
		{
			throw new ArgumentException(
				"Document type is required.",
				nameof(metadata));
		}

		if (metadata.FileSizeBytes <= 0)
		{
			throw new ArgumentOutOfRangeException(
				nameof(metadata),
				"File size must be greater than zero.");
		}

		if (metadata.Sha256Hash.Length != DocumentConstants.Sha256Length)
		{
			throw new ArgumentException(
				"SHA-256 must contain 64 hexadecimal characters.",
				nameof(metadata));
		}

		if (metadata.ExpiresOn.HasValue
			&& metadata.IssuedOn.HasValue
			&& metadata.ExpiresOn.Value < metadata.IssuedOn.Value)
		{
			throw new ArgumentException(
				"Expiry date cannot be earlier than issue date.",
				nameof(metadata));
		}
	}
}
