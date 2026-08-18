using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Persistence;

/// <summary>
/// EF-backed read persistence for SchoolEntity.
/// </summary>
public sealed class SchoolQuery(IEfMockStore store) : ISchoolQuery
{
	public Task<SchoolEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<SchoolEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<SchoolEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<SchoolEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<SchoolEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
