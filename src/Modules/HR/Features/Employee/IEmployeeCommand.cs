using System.Threading.Tasks;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Features.Employee;

/// <summary>
/// Defines command persistence operations for EmployeeEntity.
/// </summary>
public interface IEmployeeCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		EmployeeEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		EmployeeEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		EmployeeEntity entity,
		CancellationToken cancellationToken);
}
