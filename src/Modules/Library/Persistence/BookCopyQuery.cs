using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Library.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Library.Persistence;

/// <summary>
/// EF-backed read persistence for BookCopyEntity.
/// </summary>
public sealed class BookCopyQuery(IEfMockStore store) : IBookCopyQuery
{
	public Task<BookCopyEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<BookCopyEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<BookCopyEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<BookCopyEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<BookCopyEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
