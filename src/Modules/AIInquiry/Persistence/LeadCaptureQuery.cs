using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIInquiry.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIInquiry.Persistence;

/// <summary>
/// EF-backed read persistence for LeadCaptureEntity.
/// </summary>
public sealed class LeadCaptureQuery(IEfMockStore store) : ILeadCaptureQuery
{
	public Task<LeadCaptureEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<LeadCaptureEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<LeadCaptureEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<LeadCaptureEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<LeadCaptureEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
