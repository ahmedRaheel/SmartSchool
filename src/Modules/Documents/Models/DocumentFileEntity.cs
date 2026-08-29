using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;
namespace SmartSchool.Modules.Documents.Models;
public sealed class DocumentFileEntity : Entity
{
    private DocumentFileEntity() { }
    public Guid DocumentId { get; private set; } = Guid.NewGuid(); public Guid? SchoolId { get; private set; } public Guid? BranchId { get; private set; }
    public string DocumentNumber { get; private set; } = string.Empty; public string OriginalFileName { get; private set; } = string.Empty; public string StoredFileName { get; private set; } = string.Empty; public string? Extension { get; private set; }
    public string MimeType { get; private set; } = string.Empty; public long SizeBytes { get; private set; } public string Sha256 { get; private set; } = string.Empty; public string StorageProvider { get; private set; } = "DATABASE"; public byte[]? BlobData { get; private set; }
    public string Category { get; private set; } = string.Empty; public string DocumentType { get; private set; } = string.Empty; public string? Title { get; private set; } public string Status { get; private set; } = LifecycleStatuses.Active; public bool IsConfidential { get; private set; } public Guid UploadedBy { get; private set; }
    public static DocumentFileEntity Create(Guid tenantId, Guid? schoolId, Guid? branchId, string number, string original, string stored, string? extension, string mimeType, long size, string sha256, byte[] bytes, string category, string documentType, string? title, bool confidential, Guid uploadedBy) => new() { TenantId=tenantId, SchoolId=schoolId, BranchId=branchId, DocumentNumber=number, OriginalFileName=original, StoredFileName=stored, Extension=extension, MimeType=mimeType, SizeBytes=size, Sha256=sha256, BlobData=bytes, Category=category, DocumentType=documentType, Title=title, IsConfidential=confidential, UploadedBy=uploadedBy };
    public void Archive() { Status="ARCHIVED"; MarkAsUpdated(); }
}
public sealed class DocumentLinkEntity : Entity
{
    private DocumentLinkEntity() { } public Guid DocumentLinkId { get; private set; }=Guid.NewGuid(); public Guid DocumentId { get; private set; } public string EntityType { get; private set; }=string.Empty; public Guid EntityId { get; private set; } public string Purpose { get; private set; }=string.Empty; public bool IsPrimary { get; private set; }
    public static DocumentLinkEntity Create(Guid tenantId, Guid documentId, string entityType, Guid entityId, string purpose, bool isPrimary) => new() { TenantId=tenantId, DocumentId=documentId, EntityType=entityType.Trim().ToUpperInvariant(), EntityId=entityId, Purpose=purpose.Trim().ToUpperInvariant(), IsPrimary=isPrimary };
}
