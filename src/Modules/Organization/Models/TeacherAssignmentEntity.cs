using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Models;

/// <summary>
/// Represents the TeacherAssignmentEntity domain entity.
/// </summary>
public sealed class TeacherAssignmentEntity : Entity
{
    /// <summary>Gets the entity-specific identifier.</summary>
    public Guid TeacherCourseAssignmentId { get; private set; } = Guid.NewGuid();

    private TeacherAssignmentEntity()
    {
    }

    /// <summary>Gets the persisted course offering id value.</summary>
    public Guid CourseOfferingId { get; private set; }

    /// <summary>Gets the persisted employee id value.</summary>
    public Guid EmployeeId { get; private set; }

    /// <summary>Gets the persisted class section id value.</summary>
    public Guid? ClassSectionId { get; private set; }

    /// <summary>Gets the persisted teaching group id value.</summary>
    public Guid? TeachingGroupId { get; private set; }

    /// <summary>Gets the persisted assignment role value.</summary>
    public string AssignmentRole { get; private set; } = string.Empty;

    /// <summary>Gets the persisted periods per week value.</summary>
    public int? PeriodsPerWeek { get; private set; }

    /// <summary>Gets the persisted effective from value.</summary>
    public DateOnly? EffectiveFrom { get; private set; }

    /// <summary>Gets the persisted effective to value.</summary>
    public DateOnly? EffectiveTo { get; private set; }

    /// <summary>Gets the persisted is primary value.</summary>
    public bool IsPrimary { get; private set; }

    /// <summary>Gets the business code.</summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>Gets the display name.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets optional domain metadata serialized as JSON.</summary>
    public string? MetadataJson { get; private set; }

    /// <summary>Creates a new TeacherAssignmentEntity.</summary>
    /// <param name="tenantId">The owning tenant identifier.</param>
    /// <param name="code">The business code.</param>
    /// <param name="name">The display name.</param>
    /// <param name="metadataJson">Optional domain metadata.</param>
    /// <returns>The newly created entity.</returns>
    public static TeacherAssignmentEntity Create(
        Guid tenantId,
        string code,
        string name,
        string? metadataJson = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new TeacherAssignmentEntity
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
