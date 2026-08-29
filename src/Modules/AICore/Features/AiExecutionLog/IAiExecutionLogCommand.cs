using System.Threading.Tasks;
using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Features.AiExecutionLog;

/// <summary>
/// Defines command persistence operations for AiExecutionLogEntity.
/// </summary>
public interface IAiExecutionLogCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		AiExecutionLogEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		AiExecutionLogEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		AiExecutionLogEntity entity,
		CancellationToken cancellationToken);
}
