using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Workflow.Models;

/// <summary>Represents one ordered step in a workflow definition.</summary>
public sealed class WorkflowStepEntity : Entity
{
    private WorkflowStepEntity() { }

    public Guid WorkflowStepId { get; private set; } = Guid.NewGuid();
    public Guid WorkflowDefinitionId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public int StepOrder { get; private set; }
    public string StepType { get; private set; } = "APPROVAL";
    public string? ApproverRole { get; private set; }
    public string? ActionCode { get; private set; }
    public bool IsRequired { get; private set; } = true;

    public static WorkflowStepEntity Create(
        Guid tenantId,
        Guid workflowDefinitionId,
        string code,
        string name,
        int stepOrder,
        string stepType,
        string? approverRole,
        string? actionCode,
        bool isRequired)
    {
        if (stepOrder <= 0) throw new ArgumentOutOfRangeException(nameof(stepOrder));
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var type = string.IsNullOrWhiteSpace(stepType) ? "APPROVAL" : stepType.Trim().ToUpperInvariant();
        if (type == "APPROVAL" && string.IsNullOrWhiteSpace(approverRole))
            throw new ArgumentException("Approver role is required for approval steps.");

        return new WorkflowStepEntity
        {
            TenantId = tenantId,
            WorkflowDefinitionId = workflowDefinitionId,
            Code = code.Trim(),
            Name = name.Trim(),
            StepOrder = stepOrder,
            StepType = type,
            ApproverRole = Normalize(approverRole),
            ActionCode = Normalize(actionCode),
            IsRequired = isRequired
        };
    }

    public static WorkflowStepEntity Update(
       Guid tenantId,
       Guid workflowStepId,
       Guid workflowDefinitionId,
       string code,
       string name,
       int stepOrder,
       string stepType,
       string? approverRole,
       string? actionCode,
       bool isRequired)
    {
        if (stepOrder <= 0)
            throw new ArgumentOutOfRangeException(nameof(stepOrder));
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var type = string.IsNullOrWhiteSpace(stepType) ? "APPROVAL" : stepType.Trim().ToUpperInvariant();
        if (type == "APPROVAL" && string.IsNullOrWhiteSpace(approverRole))
            throw new ArgumentException("Approver role is required for approval steps.");

        return new WorkflowStepEntity
        {
            TenantId = tenantId,
            WorkflowStepId = workflowStepId,
            WorkflowDefinitionId = workflowDefinitionId,
            Code = code.Trim(),
            Name = name.Trim(),
            StepOrder = stepOrder,
            StepType = type,
            ApproverRole = Normalize(approverRole),
            ActionCode = Normalize(actionCode),
            IsRequired = isRequired
        };
    }
    private static string? Normalize(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
