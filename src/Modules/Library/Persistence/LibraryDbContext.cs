using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Library.Models;

namespace SmartSchool.Modules.Library.Persistence;

public interface ILibraryDbContext
{
	DatabaseFacade Database { get; }

	DbSet<BookCopyEntity> BookCopies { get; }
	DbSet<BookEntity> Books { get; }
	DbSet<LoanEntity> Loans { get; }
	DbSet<ReservationEntity> Reservations { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Provides strongly typed EF Core sets for this module.
/// </summary>
public sealed class LibraryDbContext(IApplicationDbContext dbContext) : ILibraryDbContext
{
	public DatabaseFacade Database => dbContext.Database;

	public DbSet<BookCopyEntity> BookCopies => dbContext.Set<BookCopyEntity>();
	public DbSet<BookEntity> Books => dbContext.Set<BookEntity>();
	public DbSet<LoanEntity> Loans => dbContext.Set<LoanEntity>();
	public DbSet<ReservationEntity> Reservations => dbContext.Set<ReservationEntity>();

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}
