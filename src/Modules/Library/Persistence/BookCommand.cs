using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Library.Models;

namespace SmartSchool.Modules.Library.Persistence;

/// <summary>
/// Executes database writes for <see cref="BookEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class BookCommand(IApplicationDbContext dbContext) : IBookCommand
{
	public async Task AddAsync(
		BookEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<BookEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		BookEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<BookEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		BookEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<BookEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
