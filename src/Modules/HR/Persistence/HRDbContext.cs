using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

public interface IHRDbContext
{
	DatabaseFacade Database { get; }

	DbSet<CandidateDocumentEntity> CandidateDocuments { get; }
	DbSet<CandidateEntity> Candidates { get; }
	DbSet<EmployeeDocumentEntity> EmployeeDocuments { get; }
	DbSet<EmployeeEducationEntity> EmployeeEducations { get; }
	DbSet<EmployeeEntity> Employees { get; }
	DbSet<EmployeeExperienceEntity> EmployeeExperiences { get; }
	DbSet<EmploymentHistoryEntity> EmploymentHistories { get; }
	DbSet<InterviewEntity> Interviews { get; }
	DbSet<JobEntity> Jobs { get; }
	DbSet<JobGradeEntity> JobGrades { get; }
	DbSet<LeaveRequestEntity> LeaveRequests { get; }
	DbSet<PayrollProfileEntity> PayrollProfiles { get; }
	DbSet<PositionEntity> Positions { get; }
	DbSet<ResumeEntity> Resumes { get; }
	DbSet<TeacherDirectoryReadEntity> TeacherDirectoryReads { get; }
	DbSet<TeacherDocumentEntity> TeacherDocuments { get; }
	DbSet<TeacherProfileEntity> TeacherProfiles { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// EF Core unit-of-work owned by the HR module.
/// This context is intentionally independent from ApplicationDbContext.
/// </summary>
public sealed class HRDbContext(DbContextOptions<HRDbContext> options)
	: DbContext(options), IHRDbContext
{
	public DbSet<CandidateDocumentEntity> CandidateDocuments => Set<CandidateDocumentEntity>();
	public DbSet<CandidateEntity> Candidates => Set<CandidateEntity>();
	public DbSet<EmployeeDocumentEntity> EmployeeDocuments => Set<EmployeeDocumentEntity>();
	public DbSet<EmployeeEducationEntity> EmployeeEducations => Set<EmployeeEducationEntity>();
	public DbSet<EmployeeEntity> Employees => Set<EmployeeEntity>();
	public DbSet<EmployeeExperienceEntity> EmployeeExperiences => Set<EmployeeExperienceEntity>();
	public DbSet<EmploymentHistoryEntity> EmploymentHistories => Set<EmploymentHistoryEntity>();
	public DbSet<InterviewEntity> Interviews => Set<InterviewEntity>();
	public DbSet<JobEntity> Jobs => Set<JobEntity>();
	public DbSet<JobGradeEntity> JobGrades => Set<JobGradeEntity>();
	public DbSet<LeaveRequestEntity> LeaveRequests => Set<LeaveRequestEntity>();
	public DbSet<PayrollProfileEntity> PayrollProfiles => Set<PayrollProfileEntity>();
	public DbSet<PositionEntity> Positions => Set<PositionEntity>();
	public DbSet<ResumeEntity> Resumes => Set<ResumeEntity>();
	public DbSet<TeacherDirectoryReadEntity> TeacherDirectoryReads => Set<TeacherDirectoryReadEntity>();
	public DbSet<TeacherDocumentEntity> TeacherDocuments => Set<TeacherDocumentEntity>();
	public DbSet<TeacherProfileEntity> TeacherProfiles => Set<TeacherProfileEntity>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(
			typeof(HRDbContext).Assembly,
			type => type.Namespace is not null
				&& type.Namespace.StartsWith("SmartSchool.Modules.HR.Persistence.Configurations", StringComparison.Ordinal));
	}
}
