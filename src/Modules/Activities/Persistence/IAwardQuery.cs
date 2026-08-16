using SmartSchool.Modules.Activities.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Activities.Persistence;

public interface IAwardQuery
{
    Task<Award?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResult<Award>> GetPageAsync(
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
