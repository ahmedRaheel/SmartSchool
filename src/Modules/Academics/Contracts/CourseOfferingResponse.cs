namespace SmartSchool.Modules.Academics.Contracts;

public sealed record CourseOfferingResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static CourseOfferingResponse FromEntity(
        Models.CourseOffering entity)
    {
        return new CourseOfferingResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
