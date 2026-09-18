using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Models;

/// <summary>
/// Represents a physical room owned by a campus.
/// </summary>
public sealed class RoomEntity : Entity
{
    private RoomEntity()
    {
    }

    /// <summary>Gets the room identifier.</summary>
    public Guid RoomId { get; private set; } = Guid.NewGuid();

    /// <summary>Gets the owning campus identifier.</summary>
    public Guid CampusId { get; private set; }

    /// <summary>Gets the business code.</summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>Gets the display name.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets the optional seating capacity.</summary>
    public int? Capacity { get; private set; }

    /// <summary>Gets the optional room type.</summary>
    public string? RoomType { get; private set; }

    /// <summary>Gets optional domain metadata serialized as JSON.</summary>
    public string? MetadataJson { get; private set; }

    /// <summary>Creates a room.</summary>
    public static RoomEntity Create(
        Guid tenantId,
        Guid campusId,
        string code,
        string name,
        int? capacity = null,
        string? roomType = null,
        string? metadataJson = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new RoomEntity
        {
            TenantId = tenantId,
            CampusId = campusId,
            Code = code.Trim(),
            Name = name.Trim(),
            Capacity = capacity,
            RoomType = roomType?.Trim(),
            MetadataJson = metadataJson
        };
    }

    /// <summary>Updates room details.</summary>
    public void UpdateDetails(
        string code,
        string name,
        int? capacity,
        string? roomType,
        string? metadataJson)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Code = code.Trim();
        Name = name.Trim();
        Capacity = capacity;
        RoomType = roomType?.Trim();
        MetadataJson = metadataJson;
        MarkAsUpdated();
    }
}
