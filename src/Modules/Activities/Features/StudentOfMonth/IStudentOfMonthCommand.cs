using System.Threading.Tasks;
using SmartSchool.Modules.Activities.Models;

namespace SmartSchool.Modules.Activities.Features.StudentOfMonth;

/// <summary>
/// Defines command persistence operations for StudentOfMonthEntity.
/// </summary>
public interface IStudentOfMonthCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		StudentOfMonthEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		StudentOfMonthEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		StudentOfMonthEntity entity,
		CancellationToken cancellationToken);
}
