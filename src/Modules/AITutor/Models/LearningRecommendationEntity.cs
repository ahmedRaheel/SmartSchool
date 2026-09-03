using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AITutor.Models;

/// <summary>
/// Represents the LearningRecommendationEntity domain entity.
/// </summary>
public sealed class LearningRecommendationEntity : Entity
{
    /// <summary>Gets the entity-specific identifier.</summary>
    public Guid LearningRecommendationId { get; private set; } = Guid.NewGuid();

    private LearningRecommendationEntity()
    {
    }

    /// <summary>Gets the persisted student id value.</summary>
    public Guid StudentId { get; private set; }

    /// <summary>Gets the persisted subject id value.</summary>
    public Guid? SubjectId { get; private set; }

    /// <summary>Gets the persisted topic value.</summary>
    public string? Topic { get; private set; }

    /// <summary>Gets the persisted recommendation type value.</summary>
    public string RecommendationType { get; private set; } = string.Empty;

    /// <summary>Gets the persisted recommendation text value.</summary>
    public string RecommendationText { get; private set; } = string.Empty;

    /// <summary>Gets the persisted priority value.</summary>
    public int Priority { get; private set; }

    /// <summary>Gets the persisted status value.</summary>
    public string Status { get; private set; } = string.Empty;

    /// <summary>Gets the business code.</summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>Gets the display name.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets optional domain metadata serialized as JSON.</summary>
    public string? MetadataJson { get; private set; }

    /// <summary>Creates a new LearningRecommendationEntity.</summary>
    /// <param name="tenantId">The owning tenant identifier.</param>
    /// <param name="code">The business code.</param>
    /// <param name="name">The display name.</param>
    /// <param name="metadataJson">Optional domain metadata.</param>
    /// <returns>The newly created entity.</returns>
    public static LearningRecommendationEntity Create(
        Guid tenantId,
        string code,
        string name,
        string? metadataJson = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new LearningRecommendationEntity
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
