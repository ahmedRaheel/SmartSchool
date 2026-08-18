using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// EF-backed read persistence for ProgramEntity.
/// </summary>
public sealed class ProgramQuery(IEfMockStore store) : IProgramQuery
{
	public Task<ProgramEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<ProgramEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<ProgramEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<ProgramEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<ProgramEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
