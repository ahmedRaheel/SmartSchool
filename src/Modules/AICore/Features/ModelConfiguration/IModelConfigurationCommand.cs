using System.Threading.Tasks;
using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Features.ModelConfiguration;

/// <summary>
/// Defines command persistence operations for ModelConfigurationEntity.
/// </summary>
public interface IModelConfigurationCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		ModelConfigurationEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		ModelConfigurationEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		ModelConfigurationEntity entity,
		CancellationToken cancellationToken);
}
