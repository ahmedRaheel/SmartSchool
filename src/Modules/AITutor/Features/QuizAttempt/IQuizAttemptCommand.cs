using System.Threading.Tasks;
using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Features.QuizAttempt;

/// <summary>
/// Defines command persistence operations for QuizAttemptEntity.
/// </summary>
public interface IQuizAttemptCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		QuizAttemptEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		QuizAttemptEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		QuizAttemptEntity entity,
		CancellationToken cancellationToken);
}
