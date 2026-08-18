using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Finance.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// EF-backed read persistence for InvoiceEntity.
/// </summary>
public sealed class InvoiceQuery(IEfMockStore store) : IInvoiceQuery
{
	public Task<InvoiceEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<InvoiceEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<InvoiceEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<InvoiceEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<InvoiceEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
