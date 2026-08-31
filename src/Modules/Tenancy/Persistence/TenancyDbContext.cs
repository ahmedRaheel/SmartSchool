using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Tenancy.Models;

namespace SmartSchool.Modules.Tenancy.Persistence;

public interface ITenancyDbContext
{
	DatabaseFacade Database { get; }

	DbSet<CampusBrandingEntity> CampusBrandings { get; }
	DbSet<SubscriptionEntity> Subscriptions { get; }
	DbSet<TenantContactEntity> TenantContacts { get; }
	DbSet<TenantEntity> Tenants { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Provides strongly typed EF Core sets for this module.
/// </summary>
public sealed class TenancyDbContext(IApplicationDbContext dbContext) : ITenancyDbContext
{
	public DatabaseFacade Database => dbContext.Database;

	public DbSet<CampusBrandingEntity> CampusBrandings => dbContext.Set<CampusBrandingEntity>();
	public DbSet<SubscriptionEntity> Subscriptions => dbContext.Set<SubscriptionEntity>();
	public DbSet<TenantContactEntity> TenantContacts => dbContext.Set<TenantContactEntity>();
	public DbSet<TenantEntity> Tenants => dbContext.Set<TenantEntity>();

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}
