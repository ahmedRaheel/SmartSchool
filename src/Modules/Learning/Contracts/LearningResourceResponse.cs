namespace SmartSchool.Modules.Learning.Contracts;

public sealed record LearningResourceResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static LearningResourceResponse FromEntity(
        Models.LearningResource entity)
    {
        return new LearningResourceResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
