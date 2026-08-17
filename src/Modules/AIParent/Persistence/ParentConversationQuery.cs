using SmartSchool.Modules.AIParent.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIParent.Persistence;

/// <summary>
/// Read-side persistence for ParentConversationEntity.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the AIParent module.
/// </summary>
public sealed class ParentConversationQuery : IParentConversationQuery
{
    public Task<ParentConversationEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentConversationEntity read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<ParentConversationEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentConversationEntity paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentConversationEntity uniqueness persistence has not been connected to the module DbContext.");
    }
}
