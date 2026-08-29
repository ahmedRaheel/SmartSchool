using System.Threading.Tasks;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Features.LeaveRequest;

/// <summary>
/// Defines command persistence operations for LeaveRequestEntity.
/// </summary>
public interface ILeaveRequestCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		LeaveRequestEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		LeaveRequestEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		LeaveRequestEntity entity,
		CancellationToken cancellationToken);
}
