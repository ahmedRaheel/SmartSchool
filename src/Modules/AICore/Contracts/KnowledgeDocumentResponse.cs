namespace SmartSchool.Modules.AICore.Contracts;

public sealed record KnowledgeDocumentResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static KnowledgeDocumentResponse FromEntity(
        Models.KnowledgeDocument entity)
    {
        return new KnowledgeDocumentResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
