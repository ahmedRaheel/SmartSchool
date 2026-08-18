using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Persistence;

/// <summary>
/// EF-backed read persistence for MessageEntity.
/// </summary>
public sealed class MessageQuery(IEfMockStore store) : IMessageQuery
{
	public Task<MessageEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<MessageEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<MessageEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<MessageEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<MessageEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
