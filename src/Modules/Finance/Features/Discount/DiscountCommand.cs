using SmartSchool.Modules.Finance.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Features.Discount;

/// <summary>
/// Executes database writes for <see cref="DiscountEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class DiscountCommand(IFinanceDbContext dbContext) : IDiscountCommand
{
	public async Task AddAsync(
		DiscountEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.Discounts
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		DiscountEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Discounts
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		DiscountEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Discounts
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
