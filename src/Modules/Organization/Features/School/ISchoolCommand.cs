using System.Threading.Tasks;
using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Features.School;

/// <summary>
/// Defines command persistence operations for SchoolEntity.
/// </summary>
public interface ISchoolCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		SchoolEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		SchoolEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		SchoolEntity entity,
		CancellationToken cancellationToken);
}
