using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Payroll.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Payroll.Persistence;

/// <summary>
/// EF-backed read persistence for IncrementEntity.
/// </summary>
public sealed class IncrementQuery(IEfMockStore store) : IIncrementQuery
{
	public Task<IncrementEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<IncrementEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<IncrementEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<IncrementEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<IncrementEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
