using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Workflow.Models;

/// <summary>Represents a pending or completed human decision for a workflow step.</summary>
public sealed class ApprovalEntity : Entity
{
    private ApprovalEntity() { }

    public Guid ApprovalId { get; private set; } = Guid.NewGuid();
    public Guid WorkflowInstanceId { get; private set; }
    public Guid WorkflowStepId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string AssignedRole { get; private set; } = string.Empty;
    public string Status { get; private set; } = "PENDING";
    public DateTimeOffset RequestedAt { get; private set; }
    public DateTimeOffset? DecisionAt { get; private set; }
    public Guid? DecidedByUserId { get; private set; }
    public string? Comments { get; private set; }

    public static ApprovalEntity Create(
        Guid tenantId,
        Guid workflowInstanceId,
        Guid workflowStepId,
        string code,
        string name,
        string assignedRole)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(assignedRole);

        return new ApprovalEntity
        {
            TenantId = tenantId,
            WorkflowInstanceId = workflowInstanceId,
            WorkflowStepId = workflowStepId,
            Code = code.Trim(),
            Name = name.Trim(),
            AssignedRole = assignedRole.Trim(),
            RequestedAt = DateTimeOffset.UtcNow
        };
    }

    public void Decide(string decision, Guid userId, string? comments)
    {
        if (!Status.Equals("PENDING", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("This approval has already been decided.");

        var normalized = decision.Trim().ToUpperInvariant();
        if (normalized is not ("APPROVED" or "REJECTED"))
            throw new ArgumentException("Decision must be APPROVED or REJECTED.");

        Status = normalized;
        DecidedByUserId = userId;
        DecisionAt = DateTimeOffset.UtcNow;
        Comments = string.IsNullOrWhiteSpace(comments) ? null : comments.Trim();
        MarkAsUpdated();
    }
}
