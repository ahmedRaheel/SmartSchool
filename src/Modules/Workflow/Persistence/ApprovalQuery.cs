using SmartSchool.Modules.Workflow.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Workflow.Persistence;

/// <summary>
/// Read-side persistence for Approval.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Workflow module.
/// </summary>
public sealed class ApprovalQuery : IApprovalQuery
{
    public Task<Approval?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Approval read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Approval>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Approval paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Approval uniqueness persistence has not been connected to the module DbContext.");
    }
}
