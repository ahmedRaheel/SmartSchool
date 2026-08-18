using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIInquiry.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIInquiry.Persistence;

/// <summary>
/// EF-backed read persistence for InquiryConversationEntity.
/// </summary>
public sealed class InquiryConversationQuery(IEfMockStore store) : IInquiryConversationQuery
{
	public Task<InquiryConversationEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<InquiryConversationEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<InquiryConversationEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<InquiryConversationEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<InquiryConversationEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
