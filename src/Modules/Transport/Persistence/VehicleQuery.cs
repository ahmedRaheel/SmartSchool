using SmartSchool.Modules.Transport.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Transport.Persistence;

/// <summary>
/// Read-side persistence for Vehicle.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Transport module.
/// </summary>
public sealed class VehicleQuery : IVehicleQuery
{
    public Task<Vehicle?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Vehicle read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Vehicle>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Vehicle paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Vehicle uniqueness persistence has not been connected to the module DbContext.");
    }
}
