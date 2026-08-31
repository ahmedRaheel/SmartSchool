using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Activities.Models;

namespace SmartSchool.Modules.Activities.Persistence;

public interface IActivitiesDbContext
{
	DatabaseFacade Database { get; }

	DbSet<ActivityEntity> Activities { get; }
	DbSet<AwardEntity> Awards { get; }
	DbSet<StudentActivityEntity> StudentActivities { get; }
	DbSet<StudentOfMonthEntity> StudentOfMonths { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Provides strongly typed EF Core sets for this module.
/// </summary>
public sealed class ActivitiesDbContext(IApplicationDbContext dbContext) : IActivitiesDbContext
{
	public DatabaseFacade Database => dbContext.Database;

	public DbSet<ActivityEntity> Activities => dbContext.Set<ActivityEntity>();
	public DbSet<AwardEntity> Awards => dbContext.Set<AwardEntity>();
	public DbSet<StudentActivityEntity> StudentActivities => dbContext.Set<StudentActivityEntity>();
	public DbSet<StudentOfMonthEntity> StudentOfMonths => dbContext.Set<StudentOfMonthEntity>();

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}
