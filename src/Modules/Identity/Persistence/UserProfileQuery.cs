using SmartSchool.Modules.Identity.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Identity.Persistence;

/// <summary>
/// Read-side persistence for UserProfile.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Identity module.
/// </summary>
public sealed class UserProfileQuery : IUserProfileQuery
{
    public Task<UserProfile?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "UserProfile read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<UserProfile>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "UserProfile paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "UserProfile uniqueness persistence has not been connected to the module DbContext.");
    }
}
