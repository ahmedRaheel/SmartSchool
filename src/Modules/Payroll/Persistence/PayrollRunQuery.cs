using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Payroll.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Payroll.Persistence;

/// <summary>
/// EF-backed read persistence for PayrollRunEntity.
/// </summary>
public sealed class PayrollRunQuery(IEfMockStore store) : IPayrollRunQuery
{
	public Task<PayrollRunEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<PayrollRunEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<PayrollRunEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<PayrollRunEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<PayrollRunEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
