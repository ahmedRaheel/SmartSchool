using SmartSchool.Modules.Admissions.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Admissions.Persistence;

/// <summary>
/// Read-side persistence for Application.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Admissions module.
/// </summary>
public sealed class ApplicationQuery : IApplicationQuery
{
    public Task<Application?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Application read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Application>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Application paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Application uniqueness persistence has not been connected to the module DbContext.");
    }
}
