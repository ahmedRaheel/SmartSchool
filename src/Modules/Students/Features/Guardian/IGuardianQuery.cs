using System.Threading.Tasks;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Features.Guardian;

/// <summary>
/// Defines query persistence operations for GuardianEntity.
/// </summary>
public interface IGuardianQuery
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<GuardianEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<PagedResult<GuardianEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<bool> ExistsByCnicNumberAsync(
        Guid tenantId,
        string cnicNumber,
        Guid? excludingId,
        CancellationToken cancellationToken);
}
