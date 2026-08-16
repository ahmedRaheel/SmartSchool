using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Persistence;

/// <summary>
/// Read-side persistence for Student.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Students module.
/// </summary>
public sealed class StudentQuery : IStudentQuery
{
    public Task<Student?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Student read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Student>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Student paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Student uniqueness persistence has not been connected to the module DbContext.");
    }
}
