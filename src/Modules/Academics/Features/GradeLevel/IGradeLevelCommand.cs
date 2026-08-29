using System.Threading.Tasks;
using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Features.GradeLevel;

/// <summary>
/// Defines command persistence operations for GradeLevelEntity.
/// </summary>
public interface IGradeLevelCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		GradeLevelEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		GradeLevelEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		GradeLevelEntity entity,
		CancellationToken cancellationToken);
}
