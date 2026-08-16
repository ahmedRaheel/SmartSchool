using SmartSchool.Modules.Documents.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Persistence;

public interface ISchoolLogoQuery
{
    Task<SchoolLogo?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResult<SchoolLogo>> GetPageAsync(
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
