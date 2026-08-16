using SmartSchool.Modules.Examinations.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Examinations.Persistence;

/// <summary>
/// Read-side persistence for StudentExamResult.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Examinations module.
/// </summary>
public sealed class StudentExamResultQuery : IStudentExamResultQuery
{
    public Task<StudentExamResult?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentExamResult read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<StudentExamResult>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentExamResult paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentExamResult uniqueness persistence has not been connected to the module DbContext.");
    }
}
