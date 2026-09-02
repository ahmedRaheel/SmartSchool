using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Application.Persistence;
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
/// Provides strongly typed EF Core sets for this module.
/// </summary>
public sealed class StudentsDbContext(IApplicationDbContext dbContext) : IStudentsDbContext
{
	public DatabaseFacade Database => dbContext.Database;

	public DbSet<AdmissionPlacementEntity> AdmissionPlacements => dbContext.Set<AdmissionPlacementEntity>();
	public DbSet<AttendanceEntity> Attendances => dbContext.Set<AttendanceEntity>();
	public DbSet<EnrollmentEntity> Enrollments => dbContext.Set<EnrollmentEntity>();
	public DbSet<GuardianEntity> Guardians => dbContext.Set<GuardianEntity>();
	public DbSet<ParentDocumentEntity> ParentDocuments => dbContext.Set<ParentDocumentEntity>();
	public DbSet<ParentProfileEntity> ParentProfiles => dbContext.Set<ParentProfileEntity>();
	public DbSet<StudentDirectoryReadEntity> StudentDirectoryReads => dbContext.Set<StudentDirectoryReadEntity>();
	public DbSet<StudentDocumentEntity> StudentDocuments => dbContext.Set<StudentDocumentEntity>();
	public DbSet<StudentEntity> Students => dbContext.Set<StudentEntity>();
	public DbSet<StudentGuardianEntity> StudentGuardians => dbContext.Set<StudentGuardianEntity>();
	public DbSet<StudentProfileEntity> StudentProfiles => dbContext.Set<StudentProfileEntity>();

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}
