using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Persistence;

public interface IStudentGuardianQuery
{
    Task<StudentGuardian?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResult<StudentGuardian>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken);
}
