using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Audit.Models;

namespace SmartSchool.Modules.Audit.Persistence;

public interface IAuditDbContext
{
	DatabaseFacade Database { get; }

	DbSet<AuditLogEntity> AuditLogs { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Provides strongly typed EF Core sets for this module.
/// </summary>
public sealed class AuditDbContext(IApplicationDbContext dbContext) : IAuditDbContext
{
	public DatabaseFacade Database => dbContext.Database;

	public DbSet<AuditLogEntity> AuditLogs => dbContext.Set<AuditLogEntity>();

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}
