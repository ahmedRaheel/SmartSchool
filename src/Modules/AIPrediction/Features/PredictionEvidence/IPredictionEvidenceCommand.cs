using System.Threading.Tasks;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Features.PredictionEvidence;

/// <summary>
/// Defines command persistence operations for PredictionEvidenceEntity.
/// </summary>
public interface IPredictionEvidenceCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		PredictionEvidenceEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		PredictionEvidenceEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		PredictionEvidenceEntity entity,
		CancellationToken cancellationToken);
}
