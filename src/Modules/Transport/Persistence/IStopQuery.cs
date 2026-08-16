using SmartSchool.Modules.Transport.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Transport.Persistence;

public interface IStopQuery
{
    Task<Stop?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResult<Stop>> GetPageAsync(
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
