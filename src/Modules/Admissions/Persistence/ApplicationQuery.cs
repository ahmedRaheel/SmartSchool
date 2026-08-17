using SmartSchool.Modules.Admissions.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Admissions.Persistence;

/// <summary>
/// Read-side persistence for ApplicationEntity.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Admissions module.
/// </summary>
public sealed class ApplicationQuery : IApplicationQuery
{
    public Task<ApplicationEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ApplicationEntity read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<ApplicationEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ApplicationEntity paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ApplicationEntity uniqueness persistence has not been connected to the module DbContext.");
    }
}
