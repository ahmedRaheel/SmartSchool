using System.Threading.Tasks;
using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Features.Term;

/// <summary>
/// Defines command persistence operations for TermEntity.
/// </summary>
public interface ITermCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		TermEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		TermEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		TermEntity entity,
		CancellationToken cancellationToken);
}
