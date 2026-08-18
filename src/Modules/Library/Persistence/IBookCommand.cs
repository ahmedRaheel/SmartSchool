using System.Threading.Tasks;
using SmartSchool.Modules.Library.Models;

namespace SmartSchool.Modules.Library.Persistence;

/// <summary>
/// Defines command persistence operations for BookEntity.
/// </summary>
public interface IBookCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		BookEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		BookEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		BookEntity entity,
		CancellationToken cancellationToken);
}
