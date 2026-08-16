using SmartSchool.Modules.Identity.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Identity.Persistence;

public interface IUserProfileQuery
{
    Task<UserProfile?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResult<UserProfile>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken);
}
