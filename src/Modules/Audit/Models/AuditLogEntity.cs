using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Audit.Models;

/// <summary>
/// Represents the AuditLogEntity domain entity.
/// </summary>
public sealed class AuditLogEntity : Entity
{
    /// <summary>Gets the entity-specific identifier.</summary>
    public long AuditLogId { get; private set; }

    private AuditLogEntity()
    {
    }

    /// <summary>Gets the persisted user id value.</summary>
    public Guid? UserId { get; private set; }

    /// <summary>Gets the persisted action value.</summary>
    public string Action { get; private set; } = string.Empty;

    /// <summary>Gets the persisted entity type value.</summary>
    public string EntityType { get; private set; } = string.Empty;

    /// <summary>Gets the persisted entity id value.</summary>
    public string? EntityId { get; private set; }

    /// <summary>Gets the persisted old values value.</summary>
    public string? OldValues { get; private set; }

    /// <summary>Gets the persisted new values value.</summary>
    public string? NewValues { get; private set; }

    /// <summary>Gets the persisted ip address value.</summary>
    public string? IpAddress { get; private set; }

    /// <summary>Gets the persisted correlation id value.</summary>
    public string? CorrelationId { get; private set; }

    /// <summary>Gets the persisted occurred at value.</summary>
    public DateTimeOffset OccurredAt { get; private set; }

    /// <summary>Gets the business code.</summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>Gets the display name.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets optional domain metadata serialized as JSON.</summary>
    public string? MetadataJson { get; private set; }

    /// <summary>Creates a new AuditLogEntity.</summary>
    /// <param name="tenantId">The owning tenant identifier.</param>
    /// <param name="code">The business code.</param>
    /// <param name="name">The display name.</param>
    /// <param name="metadataJson">Optional domain metadata.</param>
    /// <returns>The newly created entity.</returns>
    public static AuditLogEntity Create(
        Guid tenantId,
        string code,
        string name,
        string? metadataJson = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new AuditLogEntity
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
