namespace SmartSchool.Modules.Academics.Contracts;

public sealed record GradeLevelResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static GradeLevelResponse FromEntity(
        Models.GradeLevel entity)
    {
        return new GradeLevelResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
