using System.Threading.Tasks;
using SmartSchool.Modules.AIParent.Models;

namespace SmartSchool.Modules.AIParent.Features.ParentMessage;

/// <summary>
/// Defines command persistence operations for ParentMessageEntity.
/// </summary>
public interface IParentMessageCommand
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task AddAsync(
        ParentMessageEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task UpdateAsync(
        ParentMessageEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task DeleteAsync(
        ParentMessageEntity entity,
        CancellationToken cancellationToken);
}
