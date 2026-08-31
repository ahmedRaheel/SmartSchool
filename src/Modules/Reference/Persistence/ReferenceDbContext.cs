using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Reference.Models;

namespace SmartSchool.Modules.Reference.Persistence;

public interface IReferenceDbContext
{
	DatabaseFacade Database { get; }

	DbSet<LookupValueEntity> LookupValues { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Provides strongly typed EF Core sets for this module.
/// </summary>
public sealed class ReferenceDbContext(IApplicationDbContext dbContext) : IReferenceDbContext
{
	public DatabaseFacade Database => dbContext.Database;

	public DbSet<LookupValueEntity> LookupValues => dbContext.Set<LookupValueEntity>();

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}
