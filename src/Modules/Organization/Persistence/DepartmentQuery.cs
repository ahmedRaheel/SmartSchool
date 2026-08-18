using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Persistence;

/// <summary>
/// EF-backed read persistence for DepartmentEntity.
/// </summary>
public sealed class DepartmentQuery(IEfMockStore store) : IDepartmentQuery
{
	public Task<DepartmentEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<DepartmentEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<DepartmentEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<DepartmentEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<DepartmentEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
