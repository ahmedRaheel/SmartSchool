using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Admissions.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Admissions.Persistence;

/// <summary>
/// EF-backed read persistence for InquiryEntity.
/// </summary>
public sealed class InquiryQuery(IEfMockStore store) : IInquiryQuery
{
	public Task<InquiryEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<InquiryEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<InquiryEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<InquiryEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<InquiryEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
