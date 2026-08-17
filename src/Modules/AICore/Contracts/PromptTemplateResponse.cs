namespace SmartSchool.Modules.AICore.Contracts;

public sealed record PromptTemplateResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static PromptTemplateResponse FromEntity(
        Models.PromptTemplate entity)
    {
        return new PromptTemplateResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
