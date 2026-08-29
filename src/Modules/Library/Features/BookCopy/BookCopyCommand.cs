using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Library.Models;

namespace SmartSchool.Modules.Library.Features.BookCopy;

/// <summary>
/// Executes database writes for <see cref="BookCopyEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class BookCopyCommand(IApplicationDbContext dbContext) : IBookCopyCommand
{
	public async Task AddAsync(
		BookCopyEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<BookCopyEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		BookCopyEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<BookCopyEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		BookCopyEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<BookCopyEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
