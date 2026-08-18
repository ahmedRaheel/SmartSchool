using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// EF-backed read persistence for AcademicYearEntity.
/// </summary>
public sealed class AcademicYearQuery(IEfMockStore store) : IAcademicYearQuery
{
	public Task<AcademicYearEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<AcademicYearEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<AcademicYearEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<AcademicYearEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<AcademicYearEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
