using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Transport.Models;

/// <summary>
/// Represents the StopEntity domain entity.
/// </summary>
public sealed class StopEntity : Entity
{
    /// <summary>Gets the entity-specific identifier.</summary>
    public Guid StopId { get; private set; } = Guid.NewGuid();

    private StopEntity()
    {
    }

    /// <summary>Gets the business code.</summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>Gets the display name.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets optional domain metadata serialized as JSON.</summary>
    public string? MetadataJson { get; private set; }

    /// <summary>Creates a new StopEntity.</summary>
    /// <param name="tenantId">The owning tenant identifier.</param>
    /// <param name="code">The business code.</param>
    /// <param name="name">The display name.</param>
    /// <param name="metadataJson">Optional domain metadata.</param>
    /// <returns>The newly created entity.</returns>
    public static StopEntity Create(
        Guid tenantId,
        string code,
        string name,
        string? metadataJson = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new StopEntity
        {
            TenantId = tenantId,
            Code = code.Trim(),
            Name = name.Trim(),
            MetadataJson = metadataJson
        };
    }

    public Guid? RouteId { get; private set; }
    public int Sequence { get; private set; }
    public TimeOnly? PickupTime { get; private set; }
    public TimeOnly? DropoffTime { get; private set; }

    public static StopEntity Add(Guid tenantId, Guid routeId, string name, int sequence, TimeOnly pickupTime, TimeOnly dropoffTime)
    {
        var entity = Create(tenantId, Guid.NewGuid().ToString("N"), name);
        entity.RouteId = routeId;
        entity.Configure(name, sequence, pickupTime, dropoffTime);
        return entity;
    }
    public void Configure(string name, int sequence, TimeOnly pickupTime, TimeOnly dropoffTime)
    {
        Name = name.Trim(); Sequence = sequence; PickupTime = pickupTime; DropoffTime = dropoffTime;
        MarkAsUpdated();
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
