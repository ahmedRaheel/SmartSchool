using SmartSchool.Modules.Transport.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Transport.Persistence;

/// <summary>
/// Read-side persistence for Route.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Transport module.
/// </summary>
public sealed class RouteQuery : IRouteQuery
{
    public Task<Route?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Route read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Route>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Route paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Route uniqueness persistence has not been connected to the module DbContext.");
    }
}
