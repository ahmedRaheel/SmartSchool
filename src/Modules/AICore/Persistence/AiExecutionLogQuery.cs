using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// Read-side persistence for AiExecutionLog.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the AICore module.
/// </summary>
public sealed class AiExecutionLogQuery : IAiExecutionLogQuery
{
    public Task<AiExecutionLog?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AiExecutionLog read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<AiExecutionLog>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AiExecutionLog paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AiExecutionLog uniqueness persistence has not been connected to the module DbContext.");
    }
}
