using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Persistence;

/// <summary>
/// Read-side persistence for Guardian.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Students module.
/// </summary>
public sealed class GuardianQuery : IGuardianQuery
{
    public Task<Guardian?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Guardian read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Guardian>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Guardian paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Guardian uniqueness persistence has not been connected to the module DbContext.");
    }
}
