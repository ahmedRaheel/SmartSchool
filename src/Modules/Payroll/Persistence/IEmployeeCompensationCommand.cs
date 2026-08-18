using System.Threading.Tasks;
using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Persistence;

/// <summary>
/// Defines command persistence operations for EmployeeCompensationEntity.
/// </summary>
public interface IEmployeeCompensationCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		EmployeeCompensationEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		EmployeeCompensationEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		EmployeeCompensationEntity entity,
		CancellationToken cancellationToken);
}
