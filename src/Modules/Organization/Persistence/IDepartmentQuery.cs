using SmartSchool.Modules.Organization.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Persistence;

public interface IDepartmentQuery
{
    Task<Department?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResult<Department>> GetPageAsync(
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
