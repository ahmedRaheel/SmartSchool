using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// EF-backed read persistence for TimetableEntity.
/// </summary>
public sealed class TimetableQuery(IEfMockStore store) : ITimetableQuery
{
	public Task<TimetableEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<TimetableEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<TimetableEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<TimetableEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<TimetableEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
