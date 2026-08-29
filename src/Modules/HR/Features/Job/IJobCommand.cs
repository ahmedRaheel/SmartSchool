using System.Threading.Tasks;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Features.Job;

/// <summary>
/// Defines command persistence operations for JobEntity.
/// </summary>
public interface IJobCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		JobEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		JobEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		JobEntity entity,
		CancellationToken cancellationToken);
}
