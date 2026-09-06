using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence;

public interface IStudentsDbContext
{
    DatabaseFacade Database { get; }

    DbSet<AdmissionPlacementEntity> AdmissionPlacements { get; }
    DbSet<AttendanceEntity> Attendances { get; }
    DbSet<EnrollmentEntity> Enrollments { get; }
    DbSet<GuardianEntity> Guardians { get; }
    DbSet<ParentDocumentEntity> ParentDocuments { get; }
    DbSet<ParentProfileEntity> ParentProfiles { get; }
    DbSet<StudentDirectoryReadEntity> StudentDirectoryReads { get; }
    DbSet<StudentDocumentEntity> StudentDocuments { get; }
    DbSet<StudentEntity> Students { get; }
    DbSet<StudentGuardianEntity> StudentGuardians { get; }
    DbSet<StudentProfileEntity> StudentProfiles { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// EF Core unit-of-work owned by the Students module.
/// This context is intentionally independent from ApplicationDbContext.
/// </summary>
public sealed class StudentsDbContext(DbContextOptions<StudentsDbContext> options)
    : DbContext(options), IStudentsDbContext
{
    public DbSet<AdmissionPlacementEntity> AdmissionPlacements => Set<AdmissionPlacementEntity>();
    public DbSet<AttendanceEntity> Attendances => Set<AttendanceEntity>();
    public DbSet<EnrollmentEntity> Enrollments => Set<EnrollmentEntity>();
    public DbSet<GuardianEntity> Guardians => Set<GuardianEntity>();
    public DbSet<ParentDocumentEntity> ParentDocuments => Set<ParentDocumentEntity>();
    public DbSet<ParentProfileEntity> ParentProfiles => Set<ParentProfileEntity>();
    public DbSet<StudentDirectoryReadEntity> StudentDirectoryReads => Set<StudentDirectoryReadEntity>();
    public DbSet<StudentDocumentEntity> StudentDocuments => Set<StudentDocumentEntity>();
    public DbSet<StudentEntity> Students => Set<StudentEntity>();
    public DbSet<StudentGuardianEntity> StudentGuardians => Set<StudentGuardianEntity>();
    public DbSet<StudentProfileEntity> StudentProfiles => Set<StudentProfileEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(StudentsDbContext).Assembly,
            type => type.Namespace is not null
                && type.Namespace.StartsWith("SmartSchool.Modules.Students.Persistence.Configurations", StringComparison.Ordinal));
    }
}
