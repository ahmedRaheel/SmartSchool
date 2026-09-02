using SmartSchool.Modules.Inventory.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Inventory.Models;

namespace SmartSchool.Modules.Inventory.Features.StockTransaction;

/// <summary>
/// Executes database writes for <see cref="StockTransactionEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class StockTransactionCommand(IInventoryDbContext dbContext) : IStockTransactionCommand
{
	public async Task AddAsync(
		StockTransactionEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.StockTransactions
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		StockTransactionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.StockTransactions
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		StockTransactionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.StockTransactions
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
