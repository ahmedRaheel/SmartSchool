namespace SmartSchool.Modules.AITutor.Contracts;

public sealed record StudentTopicMasteryResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static StudentTopicMasteryResponse FromEntity(
        Models.StudentTopicMastery entity)
    {
        return new StudentTopicMasteryResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
