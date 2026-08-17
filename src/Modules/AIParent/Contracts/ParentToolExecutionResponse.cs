namespace SmartSchool.Modules.AIParent.Contracts;

public sealed record ParentToolExecutionResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static ParentToolExecutionResponse FromEntity(
        Models.ParentToolExecution entity)
    {
        return new ParentToolExecutionResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
