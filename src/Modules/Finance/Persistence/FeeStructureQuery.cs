using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Finance.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// EF-backed read persistence for FeeStructureEntity.
/// </summary>
public sealed class FeeStructureQuery(IEfMockStore store) : IFeeStructureQuery
{
	public Task<FeeStructureEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<FeeStructureEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<FeeStructureEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<FeeStructureEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<FeeStructureEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
