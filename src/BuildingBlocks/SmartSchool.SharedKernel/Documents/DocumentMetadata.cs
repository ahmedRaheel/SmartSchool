namespace SmartSchool.SharedKernel.Documents;

/// <summary>
/// Represents normalized metadata for a file stored outside the relational database.
/// </summary>
public sealed record DocumentMetadata(
	Guid DocumentTypeId,
	string OriginalFileName,
	string ContentType,
	long FileSizeBytes,
	string StorageProvider,
	string StorageKey,
	string Sha256Hash,
	string? DocumentNumber,
	DateOnly? IssuedOn,
	DateOnly? ExpiresOn,
	string? Notes);
