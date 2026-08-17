namespace SmartSchool.Modules.Communication.Contracts;

public sealed record ConversationParticipantResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static ConversationParticipantResponse FromEntity(
        Models.ConversationParticipant entity)
    {
        return new ConversationParticipantResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
