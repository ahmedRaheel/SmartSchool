using System.Threading.Tasks;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Features.PredictionModel;

/// <summary>
/// Defines command persistence operations for PredictionModelEntity.
/// </summary>
public interface IPredictionModelCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		PredictionModelEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		PredictionModelEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		PredictionModelEntity entity,
		CancellationToken cancellationToken);
}
