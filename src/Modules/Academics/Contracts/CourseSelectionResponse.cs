namespace SmartSchool.Modules.Academics.Contracts;

public sealed record CourseSelectionResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static CourseSelectionResponse FromEntity(
        Models.CourseSelection entity)
    {
        return new CourseSelectionResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
