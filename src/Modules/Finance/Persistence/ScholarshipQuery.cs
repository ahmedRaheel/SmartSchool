using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Finance.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// EF-backed read persistence for ScholarshipEntity.
/// </summary>
public sealed class ScholarshipQuery(IEfMockStore store) : IScholarshipQuery
{
	public Task<ScholarshipEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<ScholarshipEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<ScholarshipEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<ScholarshipEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<ScholarshipEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
