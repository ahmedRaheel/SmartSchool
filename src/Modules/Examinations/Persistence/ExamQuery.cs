using SmartSchool.Modules.Examinations.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Examinations.Persistence;

/// <summary>
/// Read-side persistence for ExamEntity.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Examinations module.
/// </summary>
public sealed class ExamQuery : IExamQuery
{
    public Task<ExamEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ExamEntity read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<ExamEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ExamEntity paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ExamEntity uniqueness persistence has not been connected to the module DbContext.");
    }
}
