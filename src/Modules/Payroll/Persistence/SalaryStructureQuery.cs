using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Payroll.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Payroll.Persistence;

/// <summary>
/// EF-backed read persistence for SalaryStructureEntity.
/// </summary>
public sealed class SalaryStructureQuery(IEfMockStore store) : ISalaryStructureQuery
{
	public Task<SalaryStructureEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<SalaryStructureEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<SalaryStructureEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<SalaryStructureEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<SalaryStructureEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
