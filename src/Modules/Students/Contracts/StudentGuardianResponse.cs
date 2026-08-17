namespace SmartSchool.Modules.Students.Contracts;

public sealed record StudentGuardianResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static StudentGuardianResponse FromEntity(
        Models.StudentGuardian entity)
    {
        return new StudentGuardianResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
