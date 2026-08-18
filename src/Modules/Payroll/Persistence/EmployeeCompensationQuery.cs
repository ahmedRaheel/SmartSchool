using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Payroll.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Payroll.Persistence;

/// <summary>
/// EF-backed read persistence for EmployeeCompensationEntity.
/// </summary>
public sealed class EmployeeCompensationQuery(IEfMockStore store) : IEmployeeCompensationQuery
{
	public Task<EmployeeCompensationEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<EmployeeCompensationEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<EmployeeCompensationEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<EmployeeCompensationEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<EmployeeCompensationEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
