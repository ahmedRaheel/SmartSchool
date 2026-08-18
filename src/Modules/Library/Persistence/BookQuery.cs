using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Library.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Library.Persistence;

/// <summary>
/// EF-backed read persistence for BookEntity.
/// </summary>
public sealed class BookQuery(IEfMockStore store) : IBookQuery
{
	public Task<BookEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<BookEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<BookEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<BookEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<BookEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
