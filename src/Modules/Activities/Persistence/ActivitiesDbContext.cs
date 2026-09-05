using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
/// EF Core unit-of-work owned by the Activities module.
/// This context is intentionally independent from ApplicationDbContext.
/// </summary>
public sealed class ActivitiesDbContext(DbContextOptions<ActivitiesDbContext> options)
    : DbContext(options), IActivitiesDbContext
{
    public DbSet<ActivityEntity> Activities => Set<ActivityEntity>();
    public DbSet<AwardEntity> Awards => Set<AwardEntity>();
    public DbSet<StudentActivityEntity> StudentActivities => Set<StudentActivityEntity>();
    public DbSet<StudentOfMonthEntity> StudentOfMonths => Set<StudentOfMonthEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ActivitiesDbContext).Assembly,
            type => type.Namespace is not null
                && type.Namespace.StartsWith("SmartSchool.Modules.Activities.Persistence.Configurations", StringComparison.Ordinal));
    }
}
