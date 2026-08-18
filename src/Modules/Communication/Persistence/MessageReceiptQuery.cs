using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Persistence;

/// <summary>
/// EF-backed read persistence for MessageReceiptEntity.
/// </summary>
public sealed class MessageReceiptQuery(IEfMockStore store) : IMessageReceiptQuery
{
	public Task<MessageReceiptEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<MessageReceiptEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<MessageReceiptEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<MessageReceiptEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<MessageReceiptEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
