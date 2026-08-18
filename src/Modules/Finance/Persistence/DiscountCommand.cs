using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// Executes database writes for <see cref="DiscountEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class DiscountCommand(IApplicationDbContext dbContext) : IDiscountCommand
{
	public async Task AddAsync(
		DiscountEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<DiscountEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		DiscountEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<DiscountEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		DiscountEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<DiscountEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
