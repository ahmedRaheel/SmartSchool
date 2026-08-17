namespace SmartSchool.Modules.Examinations.Contracts;

public sealed record ExamSubjectResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static ExamSubjectResponse FromEntity(
        Models.ExamSubject entity)
    {
        return new ExamSubjectResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
