using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIInquiry.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIInquiry.Persistence;

/// <summary>
/// EF-backed read persistence for HumanHandoffEntity.
/// </summary>
public sealed class HumanHandoffQuery(IEfMockStore store) : IHumanHandoffQuery
{
	public Task<HumanHandoffEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<HumanHandoffEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<HumanHandoffEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<HumanHandoffEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<HumanHandoffEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
