using System.Threading.Tasks;
using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Features.Subject;

/// <summary>
/// Defines command persistence operations for SubjectEntity.
/// </summary>
public interface ISubjectCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		SubjectEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		SubjectEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		SubjectEntity entity,
		CancellationToken cancellationToken);
}
