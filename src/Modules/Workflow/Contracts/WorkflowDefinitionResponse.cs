namespace SmartSchool.Modules.Workflow.Contracts;

public sealed record WorkflowDefinitionResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static WorkflowDefinitionResponse FromEntity(
        Models.WorkflowDefinition entity)
    {
        return new WorkflowDefinitionResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
