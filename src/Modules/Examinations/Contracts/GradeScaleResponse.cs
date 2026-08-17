namespace SmartSchool.Modules.Examinations.Contracts;

public sealed record GradeScaleResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static GradeScaleResponse FromEntity(
        Models.GradeScale entity)
    {
        return new GradeScaleResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
