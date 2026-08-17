namespace SmartSchool.Modules.AIPrediction.Contracts;

public sealed record ClassPerformanceInsightResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static ClassPerformanceInsightResponse FromEntity(
        Models.ClassPerformanceInsight entity)
    {
        return new ClassPerformanceInsightResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
