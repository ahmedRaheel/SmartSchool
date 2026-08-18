using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIInquiry.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIInquiry.Persistence;

/// <summary>
/// EF-backed read persistence for InquiryMessageEntity.
/// </summary>
public sealed class InquiryMessageQuery(IEfMockStore store) : IInquiryMessageQuery
{
	public Task<InquiryMessageEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<InquiryMessageEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<InquiryMessageEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<InquiryMessageEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<InquiryMessageEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
