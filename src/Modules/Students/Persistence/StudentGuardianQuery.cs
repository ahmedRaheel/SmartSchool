using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Persistence;

/// <summary>
/// Read-side persistence for StudentGuardian.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Students module.
/// </summary>
public sealed class StudentGuardianQuery : IStudentGuardianQuery
{
    public Task<StudentGuardian?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentGuardian read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<StudentGuardian>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentGuardian paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentGuardian uniqueness persistence has not been connected to the module DbContext.");
    }
}
