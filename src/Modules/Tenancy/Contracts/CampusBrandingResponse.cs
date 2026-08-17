namespace SmartSchool.Modules.Tenancy.Contracts;

public sealed record CampusBrandingResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static CampusBrandingResponse FromEntity(
        Models.CampusBranding entity)
    {
        return new CampusBrandingResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
