namespace SmartSchool.Modules.AITutor.Contracts;

public sealed record LearningRecommendationResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static LearningRecommendationResponse FromEntity(
        Models.LearningRecommendation entity)
    {
        return new LearningRecommendationResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
