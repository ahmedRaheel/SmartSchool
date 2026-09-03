using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
	DbSet<TenantSettingsEntity> TenantSettings { get; }
	DbSet<TermEntity> Terms { get; }
	DbSet<TimetableEntity> Timetables { get; }
	DbSet<TimetableEntryEntity> TimetableEntries { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// EF Core unit-of-work owned by the Organization module.
/// This context is intentionally independent from ApplicationDbContext.
/// </summary>
public sealed class OrganizationDbContext(DbContextOptions<OrganizationDbContext> options)
	: DbContext(options), IOrganizationDbContext
{
	public DbSet<AcademicSystemEntity> AcademicSystems => Set<AcademicSystemEntity>();
	public DbSet<AcademicYearEntity> AcademicYears => Set<AcademicYearEntity>();
	public DbSet<CampusBrandingEntity> CampusBrandings => Set<CampusBrandingEntity>();
	public DbSet<CampusEntity> Campuses => Set<CampusEntity>();
	public DbSet<ClassSectionEntity> ClassSections => Set<ClassSectionEntity>();
	public DbSet<CourseOfferingEntity> CourseOfferings => Set<CourseOfferingEntity>();
	public DbSet<CourseSelectionEntity> CourseSelections => Set<CourseSelectionEntity>();
	public DbSet<DepartmentEntity> Departments => Set<DepartmentEntity>();
	public DbSet<GradeLevelEntity> GradeLevels => Set<GradeLevelEntity>();
	public DbSet<ProgramEntity> Programs => Set<ProgramEntity>();
	public DbSet<SchoolDocumentEntity> SchoolDocuments => Set<SchoolDocumentEntity>();
	public DbSet<SchoolEntity> Schools => Set<SchoolEntity>();
	public DbSet<SubjectEntity> Subjects => Set<SubjectEntity>();
	public DbSet<SubscriptionEntity> Subscriptions => Set<SubscriptionEntity>();
	public DbSet<TeacherAssignmentEntity> TeacherAssignments => Set<TeacherAssignmentEntity>();
	public DbSet<TenantContactEntity> TenantContacts => Set<TenantContactEntity>();
	public DbSet<TenantEntity> Tenants => Set<TenantEntity>();
	public DbSet<TenantSettingsEntity> TenantSettings => Set<TenantSettingsEntity>();
	public DbSet<TermEntity> Terms => Set<TermEntity>();
	public DbSet<TimetableEntity> Timetables => Set<TimetableEntity>();
	public DbSet<TimetableEntryEntity> TimetableEntries => Set<TimetableEntryEntity>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(
			typeof(OrganizationDbContext).Assembly,
			type => type.Namespace is not null
				&& type.Namespace.StartsWith("SmartSchool.Modules.Organization.Persistence.Configurations", StringComparison.Ordinal));
	}
}
