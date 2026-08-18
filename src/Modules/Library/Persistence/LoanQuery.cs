using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Library.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Library.Persistence;

/// <summary>
/// EF-backed read persistence for LoanEntity.
/// </summary>
public sealed class LoanQuery(IEfMockStore store) : ILoanQuery
{
	public Task<LoanEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<LoanEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<LoanEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<LoanEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<LoanEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
