using SmartSchool.Modules.Learning.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Learning.Persistence;

/// <summary>
/// Read-side persistence for LessonEntity.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Learning module.
/// </summary>
public sealed class LessonQuery : ILessonQuery
{
    public Task<LessonEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LessonEntity read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<LessonEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LessonEntity paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LessonEntity uniqueness persistence has not been connected to the module DbContext.");
    }
}
