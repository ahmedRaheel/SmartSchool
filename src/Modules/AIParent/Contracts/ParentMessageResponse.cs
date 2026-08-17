namespace SmartSchool.Modules.AIParent.Contracts;

public sealed record ParentMessageResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static ParentMessageResponse FromEntity(
        Models.ParentMessage entity)
    {
        return new ParentMessageResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
