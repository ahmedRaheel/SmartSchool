using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// EF-backed read persistence for EmployeeEntity.
/// </summary>
public sealed class EmployeeQuery(IEfMockStore store) : IEmployeeQuery
{
	public Task<EmployeeEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<EmployeeEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<EmployeeEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<EmployeeEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<EmployeeEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
