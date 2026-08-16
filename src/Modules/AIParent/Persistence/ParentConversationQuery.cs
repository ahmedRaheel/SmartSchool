using SmartSchool.Modules.AIParent.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIParent.Persistence;

/// <summary>
/// Read-side persistence for ParentConversation.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the AIParent module.
/// </summary>
public sealed class ParentConversationQuery : IParentConversationQuery
{
    public Task<ParentConversation?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentConversation read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<ParentConversation>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentConversation paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ParentConversation uniqueness persistence has not been connected to the module DbContext.");
    }
}
