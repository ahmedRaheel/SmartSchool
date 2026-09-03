using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Modules.Reference.Models;

namespace SmartSchool.Modules.Reference.Persistence;

public interface IReferenceDbContext
{
	DatabaseFacade Database { get; }

	DbSet<LookupValueEntity> LookupValues { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// EF Core unit-of-work owned by the Reference module.
/// This context is intentionally independent from ApplicationDbContext.
/// </summary>
public sealed class ReferenceDbContext(DbContextOptions<ReferenceDbContext> options)
	: DbContext(options), IReferenceDbContext
{
	public DbSet<LookupValueEntity> LookupValues => Set<LookupValueEntity>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(
			typeof(ReferenceDbContext).Assembly,
			type => type.Namespace is not null
				&& type.Namespace.StartsWith("SmartSchool.Modules.Reference.Persistence.Configurations", StringComparison.Ordinal));
	}
}
