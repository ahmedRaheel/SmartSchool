using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Payroll.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Payroll.Persistence;

/// <summary>
/// EF-backed read persistence for PayslipEntity.
/// </summary>
public sealed class PayslipQuery(IEfMockStore store) : IPayslipQuery
{
	public Task<PayslipEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<PayslipEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<PayslipEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<PayslipEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<PayslipEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
