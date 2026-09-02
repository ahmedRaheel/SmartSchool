using SmartSchool.Modules.Library.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Library.Models;

namespace SmartSchool.Modules.Library.Features.Book;

/// <summary>
/// Executes database writes for <see cref="BookEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class BookCommand(ILibraryDbContext dbContext) : IBookCommand
{
	public async Task AddAsync(
		BookEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.Books
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		BookEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Books
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		BookEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Books
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
