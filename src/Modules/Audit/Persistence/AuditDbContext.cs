using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Modules.Audit.Models;

namespace SmartSchool.Modules.Audit.Persistence;

public interface IAuditDbContext
{
	DatabaseFacade Database { get; }

	DbSet<AuditLogEntity> AuditLogs { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// EF Core unit-of-work owned by the Audit module.
/// This context is intentionally independent from ApplicationDbContext.
/// </summary>
public sealed class AuditDbContext(DbContextOptions<AuditDbContext> options)
	: DbContext(options), IAuditDbContext
{
	public DbSet<AuditLogEntity> AuditLogs => Set<AuditLogEntity>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(
			typeof(AuditDbContext).Assembly,
			type => type.Namespace is not null
				&& type.Namespace.StartsWith("SmartSchool.Modules.Audit.Persistence.Configurations", StringComparison.Ordinal));
	}
}
