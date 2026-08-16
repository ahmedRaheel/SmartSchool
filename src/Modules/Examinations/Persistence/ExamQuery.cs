using SmartSchool.Modules.Examinations.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Examinations.Persistence;

/// <summary>
/// Read-side persistence for Exam.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Examinations module.
/// </summary>
public sealed class ExamQuery : IExamQuery
{
    public Task<Exam?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Exam read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Exam>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Exam paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Exam uniqueness persistence has not been connected to the module DbContext.");
    }
}
