using SmartSchool.Modules.Inventory.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Inventory.Models;

using SmartSchool.Modules.Inventory.Features.Item;

namespace SmartSchool.Modules.Inventory.Features.DataAccess.Item;

/// <summary>
/// Executes database writes for <see cref="ItemEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ItemCommand(IInventoryDbContext dbContext) : IItemCommand
{
	public async Task AddAsync(
		ItemEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.Items
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		ItemEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Items
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		ItemEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Items
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
