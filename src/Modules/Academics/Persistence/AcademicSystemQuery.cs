using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// EF-backed read persistence for AcademicSystemEntity.
/// </summary>
public sealed class AcademicSystemQuery(IEfMockStore store) : IAcademicSystemQuery
{
	public Task<AcademicSystemEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<AcademicSystemEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<AcademicSystemEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<AcademicSystemEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<AcademicSystemEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
