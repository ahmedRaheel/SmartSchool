using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Read-side persistence for Position.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the HR module.
/// </summary>
public sealed class PositionQuery : IPositionQuery
{
    public Task<Position?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Position read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Position>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Position paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Position uniqueness persistence has not been connected to the module DbContext.");
    }
}
