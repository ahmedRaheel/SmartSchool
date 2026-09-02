using System.Threading.Tasks;
using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Features.Discount;

/// <summary>
/// Defines command persistence operations for DiscountEntity.
/// </summary>
public interface IDiscountCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		DiscountEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		DiscountEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		DiscountEntity entity,
		CancellationToken cancellationToken);
}
