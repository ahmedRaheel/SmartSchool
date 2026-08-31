using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Persistence;

public interface IOrganizationDbContext
{
	DatabaseFacade Database { get; }

	DbSet<AcademicSystemEntity> AcademicSystems { get; }
	DbSet<AcademicYearEntity> AcademicYears { get; }
	DbSet<CampusBrandingEntity> CampusBrandings { get; }
	DbSet<CampusEntity> Campuses { get; }
	DbSet<ClassSectionEntity> ClassSections { get; }
	DbSet<CourseOfferingEntity> CourseOfferings { get; }
	DbSet<CourseSelectionEntity> CourseSelections { get; }
	DbSet<DepartmentEntity> Departments { get; }
	DbSet<GradeLevelEntity> GradeLevels { get; }
	DbSet<ProgramEntity> Programs { get; }
	DbSet<SchoolDocumentEntity> SchoolDocuments { get; }
	DbSet<SchoolEntity> Schools { get; }
	DbSet<SubjectEntity> Subjects { get; }
	DbSet<SubscriptionEntity> Subscriptions { get; }
	DbSet<TeacherAssignmentEntity> TeacherAssignments { get; }
	DbSet<TenantContactEntity> TenantContacts { get; }
	DbSet<TenantEntity> Tenants { get; }
	DbSet<TermEntity> Terms { get; }
	DbSet<TimetableEntity> Timetables { get; }
	DbSet<TimetableEntryEntity> TimetableEntries { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Provides strongly typed EF Core sets for this module.
/// </summary>
public sealed class OrganizationDbContext(IApplicationDbContext dbContext) : IOrganizationDbContext
{
	public DatabaseFacade Database => dbContext.Database;

	public DbSet<AcademicSystemEntity> AcademicSystems => dbContext.Set<AcademicSystemEntity>();
	public DbSet<AcademicYearEntity> AcademicYears => dbContext.Set<AcademicYearEntity>();
	public DbSet<CampusBrandingEntity> CampusBrandings => dbContext.Set<CampusBrandingEntity>();
	public DbSet<CampusEntity> Campuses => dbContext.Set<CampusEntity>();
	public DbSet<ClassSectionEntity> ClassSections => dbContext.Set<ClassSectionEntity>();
	public DbSet<CourseOfferingEntity> CourseOfferings => dbContext.Set<CourseOfferingEntity>();
	public DbSet<CourseSelectionEntity> CourseSelections => dbContext.Set<CourseSelectionEntity>();
	public DbSet<DepartmentEntity> Departments => dbContext.Set<DepartmentEntity>();
	public DbSet<GradeLevelEntity> GradeLevels => dbContext.Set<GradeLevelEntity>();
	public DbSet<ProgramEntity> Programs => dbContext.Set<ProgramEntity>();
	public DbSet<SchoolDocumentEntity> SchoolDocuments => dbContext.Set<SchoolDocumentEntity>();
	public DbSet<SchoolEntity> Schools => dbContext.Set<SchoolEntity>();
	public DbSet<SubjectEntity> Subjects => dbContext.Set<SubjectEntity>();
	public DbSet<SubscriptionEntity> Subscriptions => dbContext.Set<SubscriptionEntity>();
	public DbSet<TeacherAssignmentEntity> TeacherAssignments => dbContext.Set<TeacherAssignmentEntity>();
	public DbSet<TenantContactEntity> TenantContacts => dbContext.Set<TenantContactEntity>();
	public DbSet<TenantEntity> Tenants => dbContext.Set<TenantEntity>();
	public DbSet<TermEntity> Terms => dbContext.Set<TermEntity>();
	public DbSet<TimetableEntity> Timetables => dbContext.Set<TimetableEntity>();
	public DbSet<TimetableEntryEntity> TimetableEntries => dbContext.Set<TimetableEntryEntity>();

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}
