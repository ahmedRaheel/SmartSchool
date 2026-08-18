using System.Threading.Tasks;
namespace SmartSchool.Application.Documents;

/// <summary>
/// Stores document bytes outside the relational database.
/// </summary>
public interface IDocumentStorage
{
	Task<StoredDocument> SaveAsync(
		Guid tenantId,
		string ownerCategory,
		Guid ownerId,
		string originalFileName,
		string contentType,
		Stream content,
		CancellationToken cancellationToken);

	Task<Stream> OpenReadAsync(
		string storageKey,
		CancellationToken cancellationToken);

	Task DeleteAsync(
		string storageKey,
		CancellationToken cancellationToken);
}

/// <summary>Describes a successfully stored file.</summary>
public sealed record StoredDocument(
	string StorageProvider,
	string StorageKey,
	long FileSizeBytes,
	string Sha256Hash);
