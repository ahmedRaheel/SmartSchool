using System.Threading.Tasks;
using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Features.FeeType;

/// <summary>
/// Defines command persistence operations for FeeTypeEntity.
/// </summary>
public interface IFeeTypeCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		FeeTypeEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		FeeTypeEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		FeeTypeEntity entity,
		CancellationToken cancellationToken);
}
