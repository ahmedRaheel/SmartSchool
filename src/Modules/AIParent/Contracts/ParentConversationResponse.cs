namespace SmartSchool.Modules.AIParent.Contracts;

public sealed record ParentConversationResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static ParentConversationResponse FromEntity(
        Models.ParentConversation entity)
    {
        return new ParentConversationResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
