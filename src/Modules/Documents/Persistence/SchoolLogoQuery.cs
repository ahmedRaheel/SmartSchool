using SmartSchool.Modules.Documents.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Persistence;

/// <summary>
/// Read-side persistence for SchoolLogo.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Documents module.
/// </summary>
public sealed class SchoolLogoQuery : ISchoolLogoQuery
{
    public Task<SchoolLogo?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "SchoolLogo read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<SchoolLogo>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "SchoolLogo paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "SchoolLogo uniqueness persistence has not been connected to the module DbContext.");
    }
}
