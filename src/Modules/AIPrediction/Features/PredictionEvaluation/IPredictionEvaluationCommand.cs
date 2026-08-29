using System.Threading.Tasks;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Features.PredictionEvaluation;

/// <summary>
/// Defines command persistence operations for PredictionEvaluationEntity.
/// </summary>
public interface IPredictionEvaluationCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		PredictionEvaluationEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		PredictionEvaluationEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		PredictionEvaluationEntity entity,
		CancellationToken cancellationToken);
}
