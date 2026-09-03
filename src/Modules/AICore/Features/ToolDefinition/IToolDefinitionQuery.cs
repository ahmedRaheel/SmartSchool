using System.Threading.Tasks;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AICore.Features.ToolDefinition;

/// <summary>
/// Defines query persistence operations for ToolDefinitionEntity.
/// </summary>
public interface IToolDefinitionQuery
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<ToolDefinitionEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<PagedResult<ToolDefinitionEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken);
}
