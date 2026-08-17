namespace SmartSchool.Modules.Workflow.Contracts;

public sealed record WorkflowInstanceResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static WorkflowInstanceResponse FromEntity(
        Models.WorkflowInstance entity)
    {
        return new WorkflowInstanceResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
