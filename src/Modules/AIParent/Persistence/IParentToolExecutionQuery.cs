using SmartSchool.Modules.AIParent.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIParent.Persistence;

public interface IParentToolExecutionQuery
{
    Task<ParentToolExecution?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResult<ParentToolExecution>> GetPageAsync(
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
