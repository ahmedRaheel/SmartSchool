using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// Read-side persistence for ToolDefinition.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the AICore module.
/// </summary>
public sealed class ToolDefinitionQuery : IToolDefinitionQuery
{
    public Task<ToolDefinition?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ToolDefinition read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<ToolDefinition>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ToolDefinition paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ToolDefinition uniqueness persistence has not been connected to the module DbContext.");
    }
}
