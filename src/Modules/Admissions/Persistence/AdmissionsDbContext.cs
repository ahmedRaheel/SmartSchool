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
    DbSet<Features.AdmissionCriteriaWriteEntity> AdmissionCriteria { get; }
    DbSet<Features.CompleteAdmissionApplication> CompleteAdmissionApplications { get; }
    DbSet<Features.CompleteAdmissionStudent> CompleteAdmissionStudents { get; }
    DbSet<Features.CompleteAdmissionGuardian> CompleteAdmissionGuardians { get; }
    DbSet<Features.CompleteAdmissionStudentGuardian> CompleteAdmissionStudentGuardians { get; }
    DbSet<Features.CompleteAdmissionEnrollment> CompleteAdmissionEnrollments { get; }

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
    public DbSet<Features.AdmissionCriteriaWriteEntity> AdmissionCriteria => Set<Features.AdmissionCriteriaWriteEntity>();
    public DbSet<Features.CompleteAdmissionApplication> CompleteAdmissionApplications => Set<Features.CompleteAdmissionApplication>();
    public DbSet<Features.CompleteAdmissionStudent> CompleteAdmissionStudents => Set<Features.CompleteAdmissionStudent>();
    public DbSet<Features.CompleteAdmissionGuardian> CompleteAdmissionGuardians => Set<Features.CompleteAdmissionGuardian>();
    public DbSet<Features.CompleteAdmissionStudentGuardian> CompleteAdmissionStudentGuardians => Set<Features.CompleteAdmissionStudentGuardian>();
    public DbSet<Features.CompleteAdmissionEnrollment> CompleteAdmissionEnrollments => Set<Features.CompleteAdmissionEnrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AdmissionsDbContext).Assembly,
            type => type.Namespace is not null
                && type.Namespace.StartsWith("SmartSchool.Modules.Admissions.Persistence.Configurations", StringComparison.Ordinal));
    }
}
