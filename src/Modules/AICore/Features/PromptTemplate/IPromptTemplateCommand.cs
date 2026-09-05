using System.Threading.Tasks;
using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Features.PromptTemplate;

/// <summary>
/// Defines command persistence operations for PromptTemplateEntity.
/// </summary>
public interface IPromptTemplateCommand
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task AddAsync(
        PromptTemplateEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task UpdateAsync(
        PromptTemplateEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task DeleteAsync(
        PromptTemplateEntity entity,
        CancellationToken cancellationToken);
}
