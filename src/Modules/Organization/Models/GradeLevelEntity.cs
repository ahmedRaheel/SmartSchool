using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Models;

/// <summary>
/// Represents the GradeLevelEntity domain entity.
/// </summary>
public sealed class GradeLevelEntity : Entity
{
    /// <summary>Gets the entity-specific identifier.</summary>
    public Guid GradeLevelId { get; private set; } = Guid.NewGuid();

    private GradeLevelEntity()
    {
    }

    /// <summary>Gets the campus that owns this grade level.</summary>
    public Guid CampusId { get; private set; }

    /// <summary>Gets the academic system that defined this grade level.</summary>
    public Guid? AcademicSystemId { get; private set; }

    /// <summary>Gets the persisted sort order value.</summary>
    public int SortOrder { get; private set; }

    /// <summary>Gets the business code.</summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>Gets the display name.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets optional domain metadata serialized as JSON.</summary>
    public string? MetadataJson { get; private set; }

    /// <summary>Creates a new GradeLevelEntity.</summary>
    /// <param name="tenantId">The owning tenant identifier.</param>
    /// <param name="code">The business code.</param>
    /// <param name="name">The display name.</param>
    /// <param name="metadataJson">Optional domain metadata.</param>
    /// <returns>The newly created entity.</returns>
    public static GradeLevelEntity Create(
        Guid tenantId,
        Guid campusId,
        Guid? academicSystemId,
        string code,
        string name,
        int sortOrder = 0,
        string? metadataJson = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new GradeLevelEntity
        {
            TenantId = tenantId,
            CampusId = campusId,
            AcademicSystemId = academicSystemId,
            Code = code.Trim(),
            Name = name.Trim(),
            SortOrder = sortOrder,
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
