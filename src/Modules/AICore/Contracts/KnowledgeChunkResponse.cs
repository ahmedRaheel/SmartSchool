namespace SmartSchool.Modules.AICore.Contracts;

public sealed record KnowledgeChunkResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static KnowledgeChunkResponse FromEntity(
        Models.KnowledgeChunk entity)
    {
        return new KnowledgeChunkResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
