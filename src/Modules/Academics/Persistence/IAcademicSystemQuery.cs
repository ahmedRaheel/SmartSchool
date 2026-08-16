using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Persistence;

public interface IAcademicSystemQuery
{
    Task<AcademicSystem?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResult<AcademicSystem>> GetPageAsync(
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
