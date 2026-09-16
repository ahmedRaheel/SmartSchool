using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Activities.Models;

/// <summary>
/// Represents a formal student award or recognition.
/// </summary>
public sealed class AwardEntity : Entity
{
    private AwardEntity()
    {
    }

    public Guid StudentAwardId { get; private set; } = Guid.NewGuid();
    public Guid StudentId { get; private set; }
    public string AwardTypeCode { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public DateOnly AwardDate { get; private set; }
    public Guid? ApprovedBy { get; private set; }
    public Guid? DocumentId { get; private set; }

    public static AwardEntity Create(
        Guid tenantId,
        Guid studentId,
        string awardTypeCode,
        string title,
        string? description,
        DateOnly awardDate,
        Guid? approvedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(awardTypeCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        return new AwardEntity
        {
            TenantId = tenantId,
            StudentId = studentId,
            AwardTypeCode = awardTypeCode.Trim().ToUpperInvariant(),
            Title = title.Trim(),
            Description = NormalizeOptional(description),
            AwardDate = awardDate,
            ApprovedBy = approvedBy
        };
    }

    public void UpdateDetails(
        string awardTypeCode,
        string title,
        string? description,
        DateOnly awardDate,
        Guid? approvedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(awardTypeCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        AwardTypeCode = awardTypeCode.Trim().ToUpperInvariant();
        Title = title.Trim();
        Description = NormalizeOptional(description);
        AwardDate = awardDate;
        ApprovedBy = approvedBy;
        MarkAsUpdated();
    }

    public void AttachDocument(Guid documentId)
    {
        DocumentId = documentId;
        MarkAsUpdated();
    }

    private static string? NormalizeOptional(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
