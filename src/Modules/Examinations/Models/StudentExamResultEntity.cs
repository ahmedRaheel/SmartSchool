using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Examinations.Models;

/// <summary>
/// Represents the StudentExamResultEntity domain entity.
/// </summary>
public sealed class StudentExamResultEntity : Entity
{
    /// <summary>Gets the entity-specific identifier.</summary>
    public Guid StudentExamResultId { get; private set; } = Guid.NewGuid();

    private StudentExamResultEntity()
    {
    }

    /// <summary>Gets the persisted exam subject id value.</summary>
    public Guid ExamSubjectId { get; private set; }

    /// <summary>Gets the persisted student id value.</summary>
    public Guid StudentId { get; private set; }

    /// <summary>Gets the persisted marks obtained value.</summary>
    public decimal? MarksObtained { get; private set; }

    /// <summary>Gets the persisted percentage value.</summary>
    public decimal? Percentage { get; private set; }

    /// <summary>Gets the persisted grade value.</summary>
    public string? Grade { get; private set; }

    /// <summary>Gets the persisted is absent value.</summary>
    public bool IsAbsent { get; private set; }

    /// <summary>Gets the persisted remarks value.</summary>
    public string? Remarks { get; private set; }

    /// <summary>Gets the persisted entered by value.</summary>
    public Guid? EnteredBy { get; private set; }

    /// <summary>Gets the persisted verified by value.</summary>
    public Guid? VerifiedBy { get; private set; }

    /// <summary>Gets the business code.</summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>Gets the display name.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets optional domain metadata serialized as JSON.</summary>
    public string? MetadataJson { get; private set; }

    /// <summary>Creates a new StudentExamResultEntity.</summary>
    /// <param name="tenantId">The owning tenant identifier.</param>
    /// <param name="code">The business code.</param>
    /// <param name="name">The display name.</param>
    /// <param name="metadataJson">Optional domain metadata.</param>
    /// <returns>The newly created entity.</returns>
    public static StudentExamResultEntity Create(
        Guid tenantId,
        string code,
        string name,
        string? metadataJson = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new StudentExamResultEntity
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
