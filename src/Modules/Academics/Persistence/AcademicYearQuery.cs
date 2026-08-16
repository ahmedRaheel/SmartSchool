using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Read-side persistence for AcademicYear.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Academics module.
/// </summary>
public sealed class AcademicYearQuery : IAcademicYearQuery
{
    public Task<AcademicYear?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AcademicYear read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<AcademicYear>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AcademicYear paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "AcademicYear uniqueness persistence has not been connected to the module DbContext.");
    }
}
