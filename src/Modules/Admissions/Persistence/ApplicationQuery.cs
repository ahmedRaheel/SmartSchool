using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Admissions.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Admissions.Persistence;

/// <summary>
/// EF-backed read persistence for ApplicationEntity.
/// </summary>
public sealed class ApplicationQuery(IEfMockStore store) : IApplicationQuery
{
	public Task<ApplicationEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<ApplicationEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<ApplicationEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<ApplicationEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<ApplicationEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
