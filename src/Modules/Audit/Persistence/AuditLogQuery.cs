using SmartSchool.Modules.Audit.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Audit.Persistence;

/// <summary>
/// Read-side persistence for AuditLog.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Audit module.
/// </summary>
public sealed class AuditLogQuery : IAuditLogQuery
{
    public Task<AuditLog?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AuditLog read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<AuditLog>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AuditLog paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AuditLog uniqueness persistence has not been connected to the module DbContext.");
    }
}
