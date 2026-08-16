using SmartSchool.Modules.Organization.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Persistence;

/// <summary>
/// Read-side persistence for School.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Organization module.
/// </summary>
public sealed class SchoolQuery : ISchoolQuery
{
    public Task<School?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "School read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<School>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "School paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "School uniqueness persistence has not been connected to the module DbContext.");
    }
}
