using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIParent.Models;

/// <summary>
/// Represents the ParentConversationEntity domain entity.
/// </summary>
public sealed class ParentConversationEntity : Entity
{
    /// <summary>Gets the entity-specific identifier.</summary>
    public Guid ParentConversationId { get; private set; } = Guid.NewGuid();

    private ParentConversationEntity()
    {
    }

    /// <summary>Gets the persisted guardian id value.</summary>
    public Guid GuardianId { get; private set; }

    /// <summary>Gets the persisted selected student id value.</summary>
    public Guid? SelectedStudentId { get; private set; }

    /// <summary>Gets the persisted title value.</summary>
    public string? Title { get; private set; }

    /// <summary>Gets the persisted started at value.</summary>
    public DateTimeOffset StartedAt { get; private set; }

    /// <summary>Gets the persisted ended at value.</summary>
    public DateTimeOffset? EndedAt { get; private set; }

    /// <summary>Gets the persisted status value.</summary>
    public string Status { get; private set; } = string.Empty;

    /// <summary>Gets the business code.</summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>Gets the display name.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets optional domain metadata serialized as JSON.</summary>
    public string? MetadataJson { get; private set; }

    /// <summary>Creates a new ParentConversationEntity.</summary>
    /// <param name="tenantId">The owning tenant identifier.</param>
    /// <param name="code">The business code.</param>
    /// <param name="name">The display name.</param>
    /// <param name="metadataJson">Optional domain metadata.</param>
    /// <returns>The newly created entity.</returns>
    public static ParentConversationEntity Create(
        Guid tenantId,
        string code,
        string name,
        string? metadataJson = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new ParentConversationEntity
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
