using System.Threading.Tasks;
using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Features.Enrollment;

/// <summary>
/// Defines command persistence operations for EnrollmentEntity.
/// </summary>
public interface IEnrollmentCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		EnrollmentEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		EnrollmentEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		EnrollmentEntity entity,
		CancellationToken cancellationToken);
}
