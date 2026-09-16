using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Transport.Models;

/// <summary>
/// Represents the RouteEntity domain entity.
/// </summary>
public sealed class RouteEntity : Entity
{
    /// <summary>Gets the entity-specific identifier.</summary>
    public Guid RouteId { get; private set; } = Guid.NewGuid();

    private RouteEntity()
    {
    }

    /// <summary>Gets the persisted campus id value.</summary>
    public Guid CampusId { get; private set; }

    /// <summary>Gets the business code.</summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>Gets the display name.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets optional domain metadata serialized as JSON.</summary>
    public string? MetadataJson { get; private set; }

    /// <summary>Creates a new RouteEntity.</summary>
    /// <param name="tenantId">The owning tenant identifier.</param>
    /// <param name="code">The business code.</param>
    /// <param name="name">The display name.</param>
    /// <param name="metadataJson">Optional domain metadata.</param>
    /// <returns>The newly created entity.</returns>
    public static RouteEntity Create(
        Guid tenantId,
        string code,
        string name,
        string? metadataJson = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new RouteEntity
        {
            TenantId = tenantId,
            Code = code.Trim(),
            Name = name.Trim(),
            MetadataJson = metadataJson
        };
    }

    public Guid? VehicleId { get; private set; }
    public Guid? DriverId { get; private set; }
    public TimeOnly? StartTime { get; private set; }
    public TimeOnly? ArrivalTime { get; private set; }
    public TimeOnly? DismissalTime { get; private set; }

    public static RouteEntity Schedule(Guid tenantId, string code, string name, Guid campusId,
        Guid vehicleId, Guid driverId, TimeOnly startTime, TimeOnly arrivalTime, TimeOnly dismissalTime)
    {
        var entity = Create(tenantId, code, name);
        entity.Configure(name, campusId, vehicleId, driverId, startTime, arrivalTime, dismissalTime);
        return entity;
    }

    public void Configure(string name, Guid campusId, Guid vehicleId, Guid driverId,
        TimeOnly startTime, TimeOnly arrivalTime, TimeOnly dismissalTime)
    {
        Name = name.Trim(); CampusId = campusId; VehicleId = vehicleId; DriverId = driverId;
        StartTime = startTime; ArrivalTime = arrivalTime; DismissalTime = dismissalTime;
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
