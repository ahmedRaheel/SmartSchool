using SmartSchool.Modules.Identity.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Identity.Persistence;

/// <summary>
/// Read-side persistence for RoleAssignmentEntity.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Identity module.
/// </summary>
public sealed class RoleAssignmentQuery : IRoleAssignmentQuery
{
    public Task<RoleAssignmentEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "RoleAssignmentEntity read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<RoleAssignmentEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "RoleAssignmentEntity paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "RoleAssignmentEntity uniqueness persistence has not been connected to the module DbContext.");
    }
}
