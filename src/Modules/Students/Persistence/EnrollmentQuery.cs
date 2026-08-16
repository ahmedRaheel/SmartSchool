using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Persistence;

/// <summary>
/// Read-side persistence for Enrollment.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Students module.
/// </summary>
public sealed class EnrollmentQuery : IEnrollmentQuery
{
    public Task<Enrollment?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Enrollment read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Enrollment>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Enrollment paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Enrollment uniqueness persistence has not been connected to the module DbContext.");
    }
}
