using System.Threading.Tasks;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AICore.Features.PromptTemplate;

/// <summary>
/// Defines query persistence operations for PromptTemplateEntity.
/// </summary>
public interface IPromptTemplateQuery
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<PromptTemplateEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<PagedResult<PromptTemplateEntity>> GetPageAsync(
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
