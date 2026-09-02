using System.Threading.Tasks;
using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Features.TutorMessage;

/// <summary>
/// Defines command persistence operations for TutorMessageEntity.
/// </summary>
public interface ITutorMessageCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		TutorMessageEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		TutorMessageEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		TutorMessageEntity entity,
		CancellationToken cancellationToken);
}
