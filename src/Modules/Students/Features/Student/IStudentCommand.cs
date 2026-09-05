using System.Threading.Tasks;
using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Features.Student;

/// <summary>
/// Defines command persistence operations for StudentEntity.
/// </summary>
public interface IStudentCommand
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task AddAsync(
        StudentEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task UpdateAsync(
        StudentEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task DeleteAsync(
        StudentEntity entity,
        CancellationToken cancellationToken);
}
