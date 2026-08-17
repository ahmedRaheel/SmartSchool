namespace SmartSchool.Modules.AITutor.Contracts;

public sealed record TutorMessageResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static TutorMessageResponse FromEntity(
        Models.TutorMessage entity)
    {
        return new TutorMessageResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
