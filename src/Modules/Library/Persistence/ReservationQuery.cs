using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Library.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Library.Persistence;

/// <summary>
/// EF-backed read persistence for ReservationEntity.
/// </summary>
public sealed class ReservationQuery(IEfMockStore store) : IReservationQuery
{
	public Task<ReservationEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<ReservationEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<ReservationEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<ReservationEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<ReservationEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
