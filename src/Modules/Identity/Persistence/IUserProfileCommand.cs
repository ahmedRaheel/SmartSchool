using System.Threading.Tasks;
using SmartSchool.Modules.Identity.Models;

namespace SmartSchool.Modules.Identity.Persistence;

/// <summary>
/// Defines command persistence operations for UserProfileEntity.
/// </summary>
public interface IUserProfileCommand
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task AddAsync(
        UserProfileEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task UpdateAsync(
        UserProfileEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task DeleteAsync(
        UserProfileEntity entity,
        CancellationToken cancellationToken);
}
