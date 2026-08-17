namespace SmartSchool.Modules.HR.Contracts;

public sealed record JobGradeResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static JobGradeResponse FromEntity(
        Models.JobGrade entity)
    {
        return new JobGradeResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
