namespace SmartSchool.Modules.Academics.Contracts;

public sealed record AcademicYearResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static AcademicYearResponse FromEntity(
        Models.AcademicYear entity)
    {
        return new AcademicYearResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
