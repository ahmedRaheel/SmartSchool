using SmartSchool.Modules.Activities.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Activities.Persistence;

/// <summary>
/// Read-side persistence for StudentOfMonth.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Activities module.
/// </summary>
public sealed class StudentOfMonthQuery : IStudentOfMonthQuery
{
    public Task<StudentOfMonth?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentOfMonth read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<StudentOfMonth>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentOfMonth paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentOfMonth uniqueness persistence has not been connected to the module DbContext.");
    }
}
