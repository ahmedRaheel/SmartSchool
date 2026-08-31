using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

public interface IAcademicsDbContext
{
	DatabaseFacade Database { get; }

	DbSet<AcademicSystemEntity> AcademicSystems { get; }
	DbSet<AcademicYearEntity> AcademicYears { get; }
	DbSet<ClassSectionEntity> ClassSections { get; }
	DbSet<CourseOfferingEntity> CourseOfferings { get; }
	DbSet<CourseSelectionEntity> CourseSelections { get; }
	DbSet<GradeLevelEntity> GradeLevels { get; }
	DbSet<ProgramEntity> Programs { get; }
	DbSet<SubjectEntity> Subjects { get; }
	DbSet<TeacherAssignmentEntity> TeacherAssignments { get; }
	DbSet<TermEntity> Terms { get; }
	DbSet<TimetableEntity> Timetables { get; }
	DbSet<TimetableEntryEntity> TimetableEntries { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Provides strongly typed EF Core sets for this module.
/// </summary>
public sealed class AcademicsDbContext(IApplicationDbContext dbContext) : IAcademicsDbContext
{
	public DatabaseFacade Database => dbContext.Database;

	public DbSet<AcademicSystemEntity> AcademicSystems => dbContext.Set<AcademicSystemEntity>();
	public DbSet<AcademicYearEntity> AcademicYears => dbContext.Set<AcademicYearEntity>();
	public DbSet<ClassSectionEntity> ClassSections => dbContext.Set<ClassSectionEntity>();
	public DbSet<CourseOfferingEntity> CourseOfferings => dbContext.Set<CourseOfferingEntity>();
	public DbSet<CourseSelectionEntity> CourseSelections => dbContext.Set<CourseSelectionEntity>();
	public DbSet<GradeLevelEntity> GradeLevels => dbContext.Set<GradeLevelEntity>();
	public DbSet<ProgramEntity> Programs => dbContext.Set<ProgramEntity>();
	public DbSet<SubjectEntity> Subjects => dbContext.Set<SubjectEntity>();
	public DbSet<TeacherAssignmentEntity> TeacherAssignments => dbContext.Set<TeacherAssignmentEntity>();
	public DbSet<TermEntity> Terms => dbContext.Set<TermEntity>();
	public DbSet<TimetableEntity> Timetables => dbContext.Set<TimetableEntity>();
	public DbSet<TimetableEntryEntity> TimetableEntries => dbContext.Set<TimetableEntryEntity>();

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}
