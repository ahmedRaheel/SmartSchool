using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// EF-backed read persistence for TimetableEntryEntity.
/// </summary>
public sealed class TimetableEntryQuery(IEfMockStore store) : ITimetableEntryQuery
{
	public Task<TimetableEntryEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<TimetableEntryEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<TimetableEntryEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<TimetableEntryEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<TimetableEntryEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
