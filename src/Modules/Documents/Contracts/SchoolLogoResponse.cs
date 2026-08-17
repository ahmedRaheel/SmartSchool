namespace SmartSchool.Modules.Documents.Contracts;

public sealed record SchoolLogoResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static SchoolLogoResponse FromEntity(
        Models.SchoolLogo entity)
    {
        return new SchoolLogoResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
