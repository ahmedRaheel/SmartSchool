using System.Threading.Tasks;
using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Features.Stop;

/// <summary>
/// Defines command persistence operations for StopEntity.
/// </summary>
public interface IStopCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		StopEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		StopEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		StopEntity entity,
		CancellationToken cancellationToken);
}
