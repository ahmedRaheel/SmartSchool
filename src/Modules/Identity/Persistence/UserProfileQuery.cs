using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Identity.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Identity.Persistence;

/// <summary>
/// EF-backed read persistence for UserProfileEntity.
/// </summary>
public sealed class UserProfileQuery(IEfMockStore store) : IUserProfileQuery
{
	public Task<UserProfileEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<UserProfileEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<UserProfileEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<UserProfileEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<UserProfileEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
