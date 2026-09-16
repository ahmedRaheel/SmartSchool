using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Workflow.Models;

/// <summary>Represents one execution of a workflow definition for a business entity.</summary>
public sealed class WorkflowInstanceEntity : Entity
{
    private WorkflowInstanceEntity() { }

    public Guid WorkflowInstanceId { get; private set; } = Guid.NewGuid();
    public Guid WorkflowDefinitionId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string EntityType { get; private set; } = string.Empty;
    public Guid? EntityId { get; private set; }
    public string Status { get; private set; } = "IN_PROGRESS";
    public int CurrentStepOrder { get; private set; }
    public Guid StartedByUserId { get; private set; }
    public DateTimeOffset StartedAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    public string? ContextJson { get; private set; }

    public static WorkflowInstanceEntity Create(
        Guid tenantId,
        Guid workflowDefinitionId,
        string code,
        string name,
        string entityType,
        Guid? entityId,
        Guid startedByUserId,
        string? contextJson)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(entityType);

        return new WorkflowInstanceEntity
        {
            TenantId = tenantId,
            WorkflowDefinitionId = workflowDefinitionId,
            Code = code.Trim(),
            Name = name.Trim(),
            EntityType = entityType.Trim().ToUpperInvariant(),
            EntityId = entityId,
            StartedByUserId = startedByUserId,
            StartedAt = DateTimeOffset.UtcNow,
            ContextJson = string.IsNullOrWhiteSpace(contextJson) ? null : contextJson.Trim()
        };
    }

    public static WorkflowInstanceEntity Update(
       Guid tenantId,
       Guid workflowIntanceId,
       Guid workflowDefinitionId,
       string code,
       string name,
       string entityType,
       Guid? entityId,
       Guid startedByUserId,
       string? contextJson)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(entityType);

        return new WorkflowInstanceEntity
        {
            TenantId = tenantId,
            WorkflowInstanceId = workflowIntanceId,
            WorkflowDefinitionId = workflowDefinitionId,
            Code = code.Trim(),
            Name = name.Trim(),
            EntityType = entityType.Trim().ToUpperInvariant(),
            EntityId = entityId,
            StartedByUserId = startedByUserId,
            StartedAt = DateTimeOffset.UtcNow,
            ContextJson = string.IsNullOrWhiteSpace(contextJson) ? null : contextJson.Trim()
        };
    }

    public void MoveToStep(int stepOrder)
    {
        CurrentStepOrder = stepOrder;
        Status = "IN_PROGRESS";
        MarkAsUpdated();
    }

    public void Complete()
    {
        Status = "COMPLETED";
        CompletedAt = DateTimeOffset.UtcNow;
        MarkAsUpdated();
    }

    public void Reject()
    {
        Status = "REJECTED";
        CompletedAt = DateTimeOffset.UtcNow;
        MarkAsUpdated();
    }
}
