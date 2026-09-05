using System.Threading.Tasks;
using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Features.Employee;

/// <summary>
/// Defines query persistence operations for EmployeeEntity.
/// </summary>
public interface IEmployeeQuery
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<EmployeeEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<PagedResult<EmployeeEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<string?> GetBranchCodeAsync(
        Guid tenantId,
        Guid branchId,
        CancellationToken cancellationToken);

    Task<bool> ExistsByEmployeeNumberAsync(
        Guid tenantId,
        string employeeNumber,
        Guid? excludingId,
        CancellationToken cancellationToken);
}
