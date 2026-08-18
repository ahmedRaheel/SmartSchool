using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Transport.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Transport.Persistence;

/// <summary>
/// EF-backed read persistence for VehicleEntity.
/// </summary>
public sealed class VehicleQuery(IEfMockStore store) : IVehicleQuery
{
	public Task<VehicleEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<VehicleEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<VehicleEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<VehicleEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<VehicleEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
