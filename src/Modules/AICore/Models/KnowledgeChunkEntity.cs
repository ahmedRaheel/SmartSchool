using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AICore.Models;

/// <summary>
/// Represents the KnowledgeChunkEntity domain entity.
/// </summary>
public sealed class KnowledgeChunkEntity : Entity
{
    /// <summary>Gets the entity-specific identifier.</summary>
    public Guid KnowledgeChunkId { get; private set; } = Guid.NewGuid();

    private KnowledgeChunkEntity()
    {
    }

    /// <summary>Gets the persisted knowledge document id value.</summary>
    public Guid KnowledgeDocumentId { get; private set; }

    /// <summary>Gets the persisted chunk index value.</summary>
    public int ChunkIndex { get; private set; }

    /// <summary>Gets the persisted content value.</summary>
    public string Content { get; private set; } = string.Empty;

    /// <summary>Gets the persisted metadata value.</summary>
    public string? Metadata { get; private set; }

    /// <summary>Gets the persisted embedding reference value.</summary>
    public string? EmbeddingReference { get; private set; }

    /// <summary>Gets the persisted embedding value.</summary>
    public float[]? Embedding { get; private set; }

    /// <summary>Gets the business code.</summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>Gets the display name.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets optional domain metadata serialized as JSON.</summary>
    public string? MetadataJson { get; private set; }

    /// <summary>Creates a new KnowledgeChunkEntity.</summary>
    /// <param name="tenantId">The owning tenant identifier.</param>
    /// <param name="code">The business code.</param>
    /// <param name="name">The display name.</param>
    /// <param name="metadataJson">Optional domain metadata.</param>
    /// <returns>The newly created entity.</returns>
    public static KnowledgeChunkEntity Create(
        Guid tenantId,
        string code,
        string name,
        string? metadataJson = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new KnowledgeChunkEntity
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
