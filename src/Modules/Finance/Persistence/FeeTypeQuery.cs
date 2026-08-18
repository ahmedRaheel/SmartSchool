using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Finance.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// EF-backed read persistence for FeeTypeEntity.
/// </summary>
public sealed class FeeTypeQuery(IEfMockStore store) : IFeeTypeQuery
{
	public Task<FeeTypeEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<FeeTypeEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<FeeTypeEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<FeeTypeEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<FeeTypeEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
