using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Workflow.Models;

/// <summary>Defines a reusable, versioned business workflow.</summary>
public sealed class WorkflowDefinitionEntity : Entity
{
    private WorkflowDefinitionEntity() { }

    public Guid WorkflowDefinitionId { get; private set; } = Guid.NewGuid();
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string TriggerType { get; private set; } = "MANUAL";
    public string EntityType { get; private set; } = string.Empty;
    public string Status { get; private set; } = "ACTIVE";
    public int Version { get; private set; } = 1;

    public static WorkflowDefinitionEntity Create(
        Guid tenantId,
        string code,
        string name,
        string? description,
        string triggerType,
        string entityType,
        string status)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(triggerType);
        ArgumentException.ThrowIfNullOrWhiteSpace(entityType);

        return new WorkflowDefinitionEntity
        {
            TenantId = tenantId,
            Code = code.Trim(),
            Name = name.Trim(),
            Description = Normalize(description),
            TriggerType = triggerType.Trim().ToUpperInvariant(),
            EntityType = entityType.Trim().ToUpperInvariant(),
            Status = NormalizeStatus(status)
        };
    }

    public void UpdateDetails(string name, string? description, string triggerType, string entityType, string status)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(triggerType);
        ArgumentException.ThrowIfNullOrWhiteSpace(entityType);

        Name = name.Trim();
        Description = Normalize(description);
        TriggerType = triggerType.Trim().ToUpperInvariant();
        EntityType = entityType.Trim().ToUpperInvariant();
        Status = NormalizeStatus(status);
        Version++;
        MarkAsUpdated();
    }

    private static string NormalizeStatus(string value) =>
        string.Equals(value, "INACTIVE", StringComparison.OrdinalIgnoreCase) ? "INACTIVE" : "ACTIVE";

    private static string? Normalize(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
