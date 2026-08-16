using SmartSchool.Modules.Examinations.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Examinations.Persistence;

/// <summary>
/// Read-side persistence for ExamSubject.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Examinations module.
/// </summary>
public sealed class ExamSubjectQuery : IExamSubjectQuery
{
    public Task<ExamSubject?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ExamSubject read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<ExamSubject>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ExamSubject paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ExamSubject uniqueness persistence has not been connected to the module DbContext.");
    }
}
