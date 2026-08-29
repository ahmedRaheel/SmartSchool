using System.Threading.Tasks;
using SmartSchool.Modules.Activities.Models;

namespace SmartSchool.Modules.Activities.Features.Activity;

/// <summary>
/// Defines command persistence operations for ActivityEntity.
/// </summary>
public interface IActivityCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		ActivityEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		ActivityEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		ActivityEntity entity,
		CancellationToken cancellationToken);
}
