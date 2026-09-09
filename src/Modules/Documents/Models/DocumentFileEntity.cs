using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Documents.Models;

public sealed class DocumentFileEntity : Entity
{
    private DocumentFileEntity()
    {
    }

    public Guid DocumentId { get; private set; } = Guid.NewGuid();
    public Guid? CampusId { get; private set; }
    public Guid DocumentTypeId { get; private set; }
    public Guid? RequiredDocumentTypeId { get; private set; }
    public Guid OwnerId { get; private set; }
    public DocumentOwnerType OwnerType { get; private set; }
    public string DocumentNumber { get; private set; } = string.Empty;
    public string OriginalFileName { get; private set; } = string.Empty;
    public string StoredFileName { get; private set; } = string.Empty;
    public string? Extension { get; private set; }
    public string MimeType { get; private set; } = string.Empty;
    public long SizeBytes { get; private set; }
    public string Sha256 { get; private set; } = string.Empty;
    public byte[]? BlobData { get; private set; }
    public string? Title { get; private set; }
    public string Status { get; private set; } = LifecycleStatuses.Active;
    public bool IsConfidential { get; private set; }
    public Guid UploadedBy { get; private set; }

    public static DocumentFileEntity Create(
        Guid tenantId,
        Guid? campusId,
        Guid documentTypeId,
        Guid? requiredDocumentTypeId,
        Guid ownerId,
        DocumentOwnerType ownerType,
        string documentNumber,
        string originalFileName,
        string storedFileName,
        string? extension,
        string mimeType,
        long sizeBytes,
        string sha256,
        byte[] blobData,
        string? title,
        bool isConfidential,
        Guid uploadedBy)
    {
        return new DocumentFileEntity
        {
            TenantId = tenantId,
            CampusId = campusId,
            DocumentTypeId = documentTypeId,
            RequiredDocumentTypeId = requiredDocumentTypeId,
            OwnerId = ownerId,
            OwnerType = ownerType,
            DocumentNumber = documentNumber,
            OriginalFileName = originalFileName,
            StoredFileName = storedFileName,
            Extension = extension,
            MimeType = mimeType,
            SizeBytes = sizeBytes,
            Sha256 = sha256,
            BlobData = blobData,
            Title = title?.Trim(),
            IsConfidential = isConfidential,
            UploadedBy = uploadedBy
        };
    }

    public void UpdateMetadata(Guid documentTypeId, Guid? requiredDocumentTypeId, string? title, bool isConfidential)
    {
        DocumentTypeId = documentTypeId;
        RequiredDocumentTypeId = requiredDocumentTypeId;
        Title = title?.Trim();
        IsConfidential = isConfidential;
        MarkAsUpdated();
    }
}
