using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
/// EF Core unit-of-work owned by the Library module.
/// This context is intentionally independent from ApplicationDbContext.
/// </summary>
public sealed class LibraryDbContext(DbContextOptions<LibraryDbContext> options)
    : DbContext(options), ILibraryDbContext
{
    public DbSet<BookCopyEntity> BookCopies => Set<BookCopyEntity>();
    public DbSet<BookEntity> Books => Set<BookEntity>();
    public DbSet<LoanEntity> Loans => Set<LoanEntity>();
    public DbSet<ReservationEntity> Reservations => Set<ReservationEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(LibraryDbContext).Assembly,
            type => type.Namespace is not null
                && type.Namespace.StartsWith("SmartSchool.Modules.Library.Persistence.Configurations", StringComparison.Ordinal));
    }
}
