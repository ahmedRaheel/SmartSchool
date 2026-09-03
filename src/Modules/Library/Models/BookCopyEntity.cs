using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Library.Models;

/// <summary>
/// Represents the BookCopyEntity domain entity.
/// </summary>
public sealed class BookCopyEntity : Entity
{
    /// <summary>Gets the entity-specific identifier.</summary>
    public Guid BookCopyId { get; private set; } = Guid.NewGuid();

    private BookCopyEntity()
    {
    }

    /// <summary>Gets the persisted book id value.</summary>
    public Guid BookId { get; private set; }

    /// <summary>Gets the persisted campus id value.</summary>
    public Guid CampusId { get; private set; }

    /// <summary>Gets the persisted barcode value.</summary>
    public string Barcode { get; private set; } = string.Empty;

    /// <summary>Gets the persisted status value.</summary>
    public string Status { get; private set; } = string.Empty;

    /// <summary>Gets the business code.</summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>Gets the display name.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets optional domain metadata serialized as JSON.</summary>
    public string? MetadataJson { get; private set; }

    /// <summary>Creates a new BookCopyEntity.</summary>
    /// <param name="tenantId">The owning tenant identifier.</param>
    /// <param name="code">The business code.</param>
    /// <param name="name">The display name.</param>
    /// <param name="metadataJson">Optional domain metadata.</param>
    /// <returns>The newly created entity.</returns>
    public static BookCopyEntity Create(
        Guid tenantId,
        string code,
        string name,
        string? metadataJson = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new BookCopyEntity
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
