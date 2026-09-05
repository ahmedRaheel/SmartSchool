using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Modules.Admissions.Models;

namespace SmartSchool.Modules.Admissions.Persistence;

public interface IAdmissionsDbContext
{
    DatabaseFacade Database { get; }

    DbSet<AdmissionDecisionEntity> AdmissionDecisions { get; }
    DbSet<ApplicantEntity> Applicants { get; }
    DbSet<ApplicationEntity> Applications { get; }
    DbSet<InquiryEntity> Inquiries { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// EF Core unit-of-work owned by the Admissions module.
/// This context is intentionally independent from ApplicationDbContext.
/// </summary>
public sealed class AdmissionsDbContext(DbContextOptions<AdmissionsDbContext> options)
    : DbContext(options), IAdmissionsDbContext
{
    public DbSet<AdmissionDecisionEntity> AdmissionDecisions => Set<AdmissionDecisionEntity>();
    public DbSet<ApplicantEntity> Applicants => Set<ApplicantEntity>();
    public DbSet<ApplicationEntity> Applications => Set<ApplicationEntity>();
    public DbSet<InquiryEntity> Inquiries => Set<InquiryEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AdmissionsDbContext).Assembly,
            type => type.Namespace is not null
                && type.Namespace.StartsWith("SmartSchool.Modules.Admissions.Persistence.Configurations", StringComparison.Ordinal));
    }
}
