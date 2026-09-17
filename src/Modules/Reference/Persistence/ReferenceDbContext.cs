using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Modules.Reference.Models;

namespace SmartSchool.Modules.Reference.Persistence;

public interface IReferenceDbContext
{
    DatabaseFacade Database { get; }

    DbSet<LookupTypeEntity> LookupTypes { get; }
    DbSet<LookupValueEntity> LookupValues { get; }
    DbSet<CountryEntity> Countries { get; }
    DbSet<ProvinceEntity> Provinces { get; }
    DbSet<CityEntity> Cities { get; }
    DbSet<BranchGenderTypeEntity> BranchGenderTypes { get; }
    DbSet<EducationLevelEntity> EducationLevels { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// EF Core unit-of-work owned by the Reference module.
/// This context is intentionally independent from ApplicationDbContext.
/// </summary>
public sealed class ReferenceDbContext(DbContextOptions<ReferenceDbContext> options)
    : DbContext(options), IReferenceDbContext
{
    public DbSet<LookupTypeEntity> LookupTypes => Set<LookupTypeEntity>();
    public DbSet<LookupValueEntity> LookupValues => Set<LookupValueEntity>();
    public DbSet<CountryEntity> Countries => Set<CountryEntity>();
    public DbSet<ProvinceEntity> Provinces => Set<ProvinceEntity>();
    public DbSet<CityEntity> Cities => Set<CityEntity>();
    public DbSet<BranchGenderTypeEntity> BranchGenderTypes => Set<BranchGenderTypeEntity>();
    public DbSet<EducationLevelEntity> EducationLevels => Set<EducationLevelEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ReferenceDbContext).Assembly,
            type => type.Namespace is not null
                && type.Namespace.StartsWith("SmartSchool.Modules.Reference.Persistence.Configurations", StringComparison.Ordinal));
    }
}
