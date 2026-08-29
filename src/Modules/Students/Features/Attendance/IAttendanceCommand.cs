using System.Threading.Tasks;
using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Features.Attendance;

/// <summary>
/// Defines command persistence operations for AttendanceEntity.
/// </summary>
public interface IAttendanceCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		AttendanceEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		AttendanceEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		AttendanceEntity entity,
		CancellationToken cancellationToken);
}
