using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
/// EF Core unit-of-work owned by the Inventory module.
/// This context is intentionally independent from ApplicationDbContext.
/// </summary>
public sealed class InventoryDbContext(DbContextOptions<InventoryDbContext> options)
	: DbContext(options), IInventoryDbContext
{
	public DbSet<ItemEntity> Items => Set<ItemEntity>();
	public DbSet<PurchaseOrderEntity> PurchaseOrders => Set<PurchaseOrderEntity>();
	public DbSet<StockTransactionEntity> StockTransactions => Set<StockTransactionEntity>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(
			typeof(InventoryDbContext).Assembly,
			type => type.Namespace is not null
				&& type.Namespace.StartsWith("SmartSchool.Modules.Inventory.Persistence.Configurations", StringComparison.Ordinal));
	}
}
