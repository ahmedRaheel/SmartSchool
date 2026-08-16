using SmartSchool.Modules.AIParent.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIParent.Persistence;

/// <summary>
/// Read-side persistence for ParentMessage.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the AIParent module.
/// </summary>
public sealed class ParentMessageQuery : IParentMessageQuery
{
    public Task<ParentMessage?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentMessage read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<ParentMessage>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentMessage paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentMessage uniqueness persistence has not been connected to the module DbContext.");
    }
}
