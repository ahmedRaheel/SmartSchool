namespace SmartSchool.Modules.AIPrediction.Contracts;

public sealed record TeachingRecommendationResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static TeachingRecommendationResponse FromEntity(
        Models.TeachingRecommendation entity)
    {
        return new TeachingRecommendationResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
