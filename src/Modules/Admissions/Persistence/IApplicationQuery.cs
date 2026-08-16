using SmartSchool.Modules.Admissions.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Admissions.Persistence;

public interface IApplicationQuery
{
    Task<Application?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResult<Application>> GetPageAsync(
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
