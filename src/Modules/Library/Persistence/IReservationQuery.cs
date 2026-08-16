using SmartSchool.Modules.Library.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Library.Persistence;

public interface IReservationQuery
{
    Task<Reservation?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResult<Reservation>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken);
}
