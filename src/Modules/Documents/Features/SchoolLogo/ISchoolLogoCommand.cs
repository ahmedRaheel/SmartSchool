using System.Threading.Tasks;
using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Features.SchoolLogo;

/// <summary>
/// Defines command persistence operations for SchoolLogoEntity.
/// </summary>
public interface ISchoolLogoCommand
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task AddAsync(
        SchoolLogoEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task UpdateAsync(
        SchoolLogoEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task DeleteAsync(
        SchoolLogoEntity entity,
        CancellationToken cancellationToken);
}
