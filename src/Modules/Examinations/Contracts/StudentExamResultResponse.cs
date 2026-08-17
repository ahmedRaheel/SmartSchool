namespace SmartSchool.Modules.Examinations.Contracts;

public sealed record StudentExamResultResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static StudentExamResultResponse FromEntity(
        Models.StudentExamResult entity)
    {
        return new StudentExamResultResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
