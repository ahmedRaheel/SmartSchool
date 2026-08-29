using System.Threading.Tasks;
using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Features.Timetable;

/// <summary>
/// Defines command persistence operations for TimetableEntity.
/// </summary>
public interface ITimetableCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		TimetableEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		TimetableEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		TimetableEntity entity,
		CancellationToken cancellationToken);
}
