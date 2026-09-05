using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Models;

/// <summary>
/// Represents the DepartmentEntity domain entity.
/// </summary>
public sealed class DepartmentEntity : Entity
{
    /// <summary>Gets the entity-specific identifier.</summary>
    public Guid DepartmentId { get; private set; } = Guid.NewGuid();

    private DepartmentEntity()
    {
    }

    /// <summary>Gets the persisted campus id value.</summary>
    public Guid? CampusId { get; private set; }

    /// <summary>Gets the employee assigned as head of department.</summary>
    public Guid? HeadOfDepartmentEmployeeId { get; private set; }

    /// <summary>Gets the business code.</summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>Gets the display name.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets the department telephone number.</summary>
    public string? Telephone { get; private set; }

    /// <summary>Gets the department email address.</summary>
    public string? Email { get; private set; }

    /// <summary>Gets optional domain metadata serialized as JSON.</summary>
    public string? MetadataJson { get; private set; }

    /// <summary>Creates a new DepartmentEntity.</summary>
    /// <param name="tenantId">The owning tenant identifier.</param>
    /// <param name="code">The business code.</param>
    /// <param name="name">The display name.</param>
    /// <param name="metadataJson">Optional domain metadata.</param>
    /// <returns>The newly created entity.</returns>
    public static DepartmentEntity Create(
        Guid tenantId,
        Guid campusId,
        Guid? headOfDepartmentEmployeeId,
        string code,
        string name,
        string? telephone,
        string? email,
        string? metadataJson = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new DepartmentEntity
        {
            TenantId = tenantId,
            CampusId = campusId,
            HeadOfDepartmentEmployeeId = headOfDepartmentEmployeeId,
            Code = code.Trim(),
            Name = name.Trim(),
            Telephone = string.IsNullOrWhiteSpace(telephone) ? null : telephone.Trim(),
            Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim(),
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
        string? telephone,
        string? email,
        string? metadataJson = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Code = code.Trim();
        Name = name.Trim();
        Telephone = string.IsNullOrWhiteSpace(telephone) ? null : telephone.Trim();
        Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();
        MetadataJson = metadataJson;
        MarkAsUpdated();
    }
}
