using System.Threading.Tasks;
using SmartSchool.Modules.Admissions.Models;

namespace SmartSchool.Modules.Admissions.Persistence;

/// <summary>
/// Defines command persistence operations for AdmissionDecisionEntity.
/// </summary>
public interface IAdmissionDecisionCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		AdmissionDecisionEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		AdmissionDecisionEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		AdmissionDecisionEntity entity,
		CancellationToken cancellationToken);
}
