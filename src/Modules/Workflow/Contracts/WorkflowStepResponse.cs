namespace SmartSchool.Modules.Workflow.Contracts;

public sealed record WorkflowStepResponse(
    Guid Id,
    string Code,
    string Name,
    bool IsActive,
    string? MetadataJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt)
{
    public static WorkflowStepResponse FromEntity(
        Models.WorkflowStep entity)
    {
        return new WorkflowStepResponse(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.IsActive,
            entity.MetadataJson,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}
