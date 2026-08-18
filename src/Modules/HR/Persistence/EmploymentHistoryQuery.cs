using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// EF-backed read persistence for EmploymentHistoryEntity.
/// </summary>
public sealed class EmploymentHistoryQuery(IEfMockStore store) : IEmploymentHistoryQuery
{
	public Task<EmploymentHistoryEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<EmploymentHistoryEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<EmploymentHistoryEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<EmploymentHistoryEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<EmploymentHistoryEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
