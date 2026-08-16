using SmartSchool.Modules.Examinations.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Examinations.Persistence;

/// <summary>
/// Read-side persistence for GradeScale.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Examinations module.
/// </summary>
public sealed class GradeScaleQuery : IGradeScaleQuery
{
    public Task<GradeScale?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GradeScale read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<GradeScale>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GradeScale paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GradeScale uniqueness persistence has not been connected to the module DbContext.");
    }
}
