namespace SmartSchool.Modules.Academics.Contracts;

public sealed record TimetableEntryResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static TimetableEntryResponse FromEntity(
        Models.TimetableEntry entity)
    {
        return new TimetableEntryResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
