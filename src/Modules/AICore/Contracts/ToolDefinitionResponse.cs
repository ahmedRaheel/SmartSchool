namespace SmartSchool.Modules.AICore.Contracts;

public sealed record ToolDefinitionResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static ToolDefinitionResponse FromEntity(
        Models.ToolDefinition entity)
    {
        return new ToolDefinitionResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
