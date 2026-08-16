using SmartSchool.Modules.Library.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Library.Persistence;

/// <summary>
/// Read-side persistence for Reservation.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Library module.
/// </summary>
public sealed class ReservationQuery : IReservationQuery
{
    public Task<Reservation?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Reservation read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Reservation>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Reservation paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Reservation uniqueness persistence has not been connected to the module DbContext.");
    }
}
