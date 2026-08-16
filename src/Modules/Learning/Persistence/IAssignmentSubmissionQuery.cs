using SmartSchool.Modules.Learning.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Learning.Persistence;

public interface IAssignmentSubmissionQuery
{
    Task<AssignmentSubmission?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResult<AssignmentSubmission>> GetPageAsync(
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
