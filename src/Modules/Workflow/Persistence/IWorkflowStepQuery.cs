using SmartSchool.Modules.Workflow.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Workflow.Persistence;

public interface IWorkflowStepQuery
{
    Task<WorkflowStep?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResult<WorkflowStep>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken);
}
