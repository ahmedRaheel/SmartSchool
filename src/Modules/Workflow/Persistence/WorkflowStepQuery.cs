using SmartSchool.Modules.Workflow.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Workflow.Persistence;

/// <summary>
/// Read-side persistence for WorkflowStepEntity.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Workflow module.
/// </summary>
public sealed class WorkflowStepQuery : IWorkflowStepQuery
{
    public Task<WorkflowStepEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "WorkflowStepEntity read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<WorkflowStepEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "WorkflowStepEntity paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "WorkflowStepEntity uniqueness persistence has not been connected to the module DbContext.");
    }
}
