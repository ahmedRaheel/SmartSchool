namespace SmartSchool.Modules.AIPrediction.Contracts;

public sealed record TopicPerformanceInsightResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static TopicPerformanceInsightResponse FromEntity(
        Models.TopicPerformanceInsight entity)
    {
        return new TopicPerformanceInsightResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
