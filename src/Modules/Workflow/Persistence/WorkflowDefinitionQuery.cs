using SmartSchool.Modules.Workflow.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Workflow.Persistence;

/// <summary>
/// Read-side persistence for WorkflowDefinition.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Workflow module.
/// </summary>
public sealed class WorkflowDefinitionQuery : IWorkflowDefinitionQuery
{
    public Task<WorkflowDefinition?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "WorkflowDefinition read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<WorkflowDefinition>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "WorkflowDefinition paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "WorkflowDefinition uniqueness persistence has not been connected to the module DbContext.");
    }
}
