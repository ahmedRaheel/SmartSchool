using System.Threading.Tasks;
using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Features.TimetableEntry;

/// <summary>
/// Defines command persistence operations for TimetableEntryEntity.
/// </summary>
public interface ITimetableEntryCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		TimetableEntryEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		TimetableEntryEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		TimetableEntryEntity entity,
		CancellationToken cancellationToken);
}
