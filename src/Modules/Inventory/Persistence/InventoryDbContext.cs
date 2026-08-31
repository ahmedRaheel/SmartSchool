using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Inventory.Models;

namespace SmartSchool.Modules.Inventory.Persistence;

public interface IInventoryDbContext
{
	DatabaseFacade Database { get; }

	DbSet<ItemEntity> Items { get; }
	DbSet<PurchaseOrderEntity> PurchaseOrders { get; }
	DbSet<StockTransactionEntity> StockTransactions { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Provides strongly typed EF Core sets for this module.
/// </summary>
public sealed class InventoryDbContext(IApplicationDbContext dbContext) : IInventoryDbContext
{
	public DatabaseFacade Database => dbContext.Database;

	public DbSet<ItemEntity> Items => dbContext.Set<ItemEntity>();
	public DbSet<PurchaseOrderEntity> PurchaseOrders => dbContext.Set<PurchaseOrderEntity>();
	public DbSet<StockTransactionEntity> StockTransactions => dbContext.Set<StockTransactionEntity>();

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}
