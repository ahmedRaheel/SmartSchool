namespace SmartSchool.Modules.Academics.Contracts;

public sealed record AcademicSystemResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static AcademicSystemResponse FromEntity(
        Models.AcademicSystem entity)
    {
        return new AcademicSystemResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
