using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// EF-backed read persistence for PositionEntity.
/// </summary>
public sealed class PositionQuery(IEfMockStore store) : IPositionQuery
{
	public Task<PositionEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<PositionEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<PositionEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<PositionEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<PositionEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
