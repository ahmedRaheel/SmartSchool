using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Finance.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// EF-backed read persistence for PaymentEntity.
/// </summary>
public sealed class PaymentQuery(IEfMockStore store) : IPaymentQuery
{
	public Task<PaymentEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<PaymentEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<PaymentEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<PaymentEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<PaymentEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
