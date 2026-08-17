namespace SmartSchool.Modules.AICore.Contracts;

public sealed record KnowledgeCollectionResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static KnowledgeCollectionResponse FromEntity(
        Models.KnowledgeCollection entity)
    {
        return new KnowledgeCollectionResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
