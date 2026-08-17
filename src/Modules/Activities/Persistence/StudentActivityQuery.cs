using SmartSchool.Modules.Activities.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Activities.Persistence;

/// <summary>
/// Read-side persistence for StudentActivityEntity.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Activities module.
/// </summary>
public sealed class StudentActivityQuery : IStudentActivityQuery
{
    public Task<StudentActivityEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentActivityEntity read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<StudentActivityEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentActivityEntity paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentActivityEntity uniqueness persistence has not been connected to the module DbContext.");
    }
}
