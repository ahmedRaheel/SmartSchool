using System.Threading.Tasks;
using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Features.Guardian;

/// <summary>
/// Defines command persistence operations for GuardianEntity.
/// </summary>
public interface IGuardianCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		GuardianEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		GuardianEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		GuardianEntity entity,
		CancellationToken cancellationToken);
}
