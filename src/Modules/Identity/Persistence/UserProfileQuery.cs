using SmartSchool.Modules.Identity.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Identity.Persistence;

/// <summary>
/// Read-side persistence for UserProfileEntity.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Identity module.
/// </summary>
public sealed class UserProfileQuery : IUserProfileQuery
{
    public Task<UserProfileEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "UserProfileEntity read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<UserProfileEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "UserProfileEntity paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "UserProfileEntity uniqueness persistence has not been connected to the module DbContext.");
    }
}
