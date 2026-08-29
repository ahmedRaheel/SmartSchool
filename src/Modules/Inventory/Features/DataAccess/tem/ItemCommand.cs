using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Inventory.Models;

using SmartSchool.Modules.Inventory.Features.Item;

namespace SmartSchool.Modules.Inventory.Features.DataAccess.tem;

/// <summary>
/// Executes database writes for <see cref="ItemEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ItemCommand(IApplicationDbContext dbContext) : IItemCommand
{
	public async Task AddAsync(
		ItemEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<ItemEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		ItemEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ItemEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		ItemEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ItemEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
