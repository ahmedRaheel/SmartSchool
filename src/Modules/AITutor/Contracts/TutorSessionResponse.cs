namespace SmartSchool.Modules.AITutor.Contracts;

public sealed record TutorSessionResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static TutorSessionResponse FromEntity(
        Models.TutorSession entity)
    {
        return new TutorSessionResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
