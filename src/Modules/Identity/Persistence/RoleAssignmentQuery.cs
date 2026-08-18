using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Identity.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Identity.Persistence;

/// <summary>
/// EF-backed read persistence for RoleAssignmentEntity.
/// </summary>
public sealed class RoleAssignmentQuery(IEfMockStore store) : IRoleAssignmentQuery
{
	public Task<RoleAssignmentEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<RoleAssignmentEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<RoleAssignmentEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<RoleAssignmentEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<RoleAssignmentEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
