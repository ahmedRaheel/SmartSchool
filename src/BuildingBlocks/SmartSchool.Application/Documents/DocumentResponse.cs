namespace SmartSchool.Application.Documents;

/// <summary>
/// Represents safe document metadata returned by detail APIs.
/// File content and internal storage keys are intentionally excluded.
/// </summary>
public sealed record DocumentResponse(
    Guid Id,
    Guid DocumentTypeId,
    string OriginalFileName,
    string ContentType,
    long FileSizeBytes,
    bool IsVerified,
    DateOnly? IssuedOn,
    DateOnly? ExpiresOn);
