using System.Threading.Tasks;
using SmartSchool.Modules.Examinations.Models;

namespace SmartSchool.Modules.Examinations.Features.ExamSubject;

/// <summary>
/// Defines command persistence operations for ExamSubjectEntity.
/// </summary>
public interface IExamSubjectCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		ExamSubjectEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		ExamSubjectEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		ExamSubjectEntity entity,
		CancellationToken cancellationToken);
}
