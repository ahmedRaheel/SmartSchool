using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Inventory.Models;

namespace SmartSchool.Modules.Inventory.Persistence;

/// <summary>
/// Executes database writes for <see cref="StockTransactionEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class StockTransactionCommand(IApplicationDbContext dbContext) : IStockTransactionCommand
{
	public async Task AddAsync(
		StockTransactionEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<StockTransactionEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		StockTransactionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<StockTransactionEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		StockTransactionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<StockTransactionEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
