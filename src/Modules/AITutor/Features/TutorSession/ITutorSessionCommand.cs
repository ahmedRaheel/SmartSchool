using System.Threading.Tasks;
using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Features.TutorSession;

/// <summary>
/// Defines command persistence operations for TutorSessionEntity.
/// </summary>
public interface ITutorSessionCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		TutorSessionEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		TutorSessionEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		TutorSessionEntity entity,
		CancellationToken cancellationToken);
}
