using SmartSchool.Modules.Audit.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Audit.Persistence;

public interface IAuditLogQuery
{
    Task<AuditLog?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResult<AuditLog>> GetPageAsync(
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
