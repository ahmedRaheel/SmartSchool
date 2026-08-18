using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Documents.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Persistence;

/// <summary>
/// EF-backed read persistence for SchoolLogoEntity.
/// </summary>
public sealed class SchoolLogoQuery(IEfMockStore store) : ISchoolLogoQuery
{
	public Task<SchoolLogoEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<SchoolLogoEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<SchoolLogoEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<SchoolLogoEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<SchoolLogoEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
