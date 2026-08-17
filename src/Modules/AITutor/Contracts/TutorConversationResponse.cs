namespace SmartSchool.Modules.AITutor.Contracts;

public sealed record TutorConversationResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static TutorConversationResponse FromEntity(
        Models.TutorConversation entity)
    {
        return new TutorConversationResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
