using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Transport.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Transport.Persistence;

/// <summary>
/// EF-backed read persistence for RouteEntity.
/// </summary>
public sealed class RouteQuery(IEfMockStore store) : IRouteQuery
{
	public Task<RouteEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<RouteEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<RouteEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<RouteEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<RouteEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
